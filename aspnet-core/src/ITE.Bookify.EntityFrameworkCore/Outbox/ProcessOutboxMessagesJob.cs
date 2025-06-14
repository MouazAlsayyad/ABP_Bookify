using ITE.Bookify.Abstractions;
using ITE.Bookify.Clock;
using ITE.Bookify.EntityFrameworkCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITE.Bookify.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly BookifyDbContextFactory _dbContextFactory;
    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly OutboxOptions _outboxOptions;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        BookifyDbContextFactory dbContextFactory,
        IPublisher publisher,
        IDateTimeProvider dateTimeProvider,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _dbContextFactory = dbContextFactory;
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
        _outboxOptions = outboxOptions.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Beginning to process outbox messages");

        await using var dbContext = _dbContextFactory.CreateDbContext(Array.Empty<string>());
        await using var transaction = await dbContext.Database.BeginTransactionAsync(context.CancellationToken);

        try
        {
            var outboxMessages = await GetOutboxMessagesAsync(dbContext);

            foreach (var outboxMessage in outboxMessages)
            {
                Exception? exception = null;
                IDomainEvent? domainEvent = null;
                try
                {
                    domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                        outboxMessage.Content,
                        JsonSerializerSettings)!;
                    if (domainEvent is null)
                    {
                        // This specific case might be a "poison pill" if JSON is malformed
                        _logger.LogError("Failed to deserialize outbox message {MessageId} to IDomainEvent. Content was: {Content}",
                            outboxMessage.Id, outboxMessage.Content);
                        exception = new JsonSerializationException($"Deserialized event was null for message {outboxMessage.Id}.");
                        // Decide if this message should be retried or immediately dead-lettered
                    }
                    else
                    {
                        await _publisher.Publish(domainEvent, context.CancellationToken);
                    }
                }
                catch (Exception caughtException)
                {
                    _logger.LogError(
                        caughtException,
                        "Exception while processing outbox message {MessageId}",
                        outboxMessage.Id);

                    exception = caughtException;
                }

                await UpdateOutboxMessageAsync(dbContext, outboxMessage, exception);
            }

            await dbContext.SaveChangesAsync(context.CancellationToken);
            await transaction.CommitAsync(context.CancellationToken);

            _logger.LogInformation("Completed processing outbox messages");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing outbox messages");
            await transaction.RollbackAsync(context.CancellationToken);
            throw;
        }
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(BookifyDbContext dbContext)
    {
        return await dbContext.OutboxMessages
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(_outboxOptions.BatchSize)
            .Select(x => new OutboxMessageResponse(x.Id, x.Content))
            .ToListAsync();
    }

    private async Task UpdateOutboxMessageAsync(
    BookifyDbContext dbContext,
    OutboxMessageResponse outboxMessageResponse,
    Exception? exception)
    {
        var message = await dbContext.OutboxMessages.FindAsync(outboxMessageResponse.Id);
        if (message != null)
        {
            message.ProcessedOnUtc = _dateTimeProvider.UtcNow;
            message.Error = exception?.ToString();
        }
    }

    internal sealed record OutboxMessageResponse(Guid Id, string Content);
}
