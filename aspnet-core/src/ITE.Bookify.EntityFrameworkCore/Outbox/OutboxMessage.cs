using System;

namespace ITE.Bookify.Outbox;
public sealed class OutboxMessage
{
    private OutboxMessage() { }
    public OutboxMessage(Guid id, DateTime occurredOnUtc, string type, string content)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
        Content = content;
        Type = type;
    }

    public Guid Id { get; set; }

    public DateTime OccurredOnUtc { get; set; }

    public string Type { get; set; }

    public string Content { get; set; }

    public DateTime? ProcessedOnUtc { get; set; }

    public string? Error { get; set; }

    public void MarkAsProcessed(DateTime processedOnUtc)
    {
        ProcessedOnUtc = processedOnUtc;
        Error = null;
    }

    public void MarkAsFailed(DateTime processedOnUtc, string error)
    {
        ProcessedOnUtc = processedOnUtc;
        Error = error;
    }
}
