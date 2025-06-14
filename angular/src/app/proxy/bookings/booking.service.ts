import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { BookingResponse } from '../booking/get-booking/models';
import type { ReserveBookingDto } from '../booking/reserve-booking/models';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  apiName = 'Default';
  

  cancelBookingByIdAndCancellationToken = (id: string, cancellationToken: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/booking/${id}/cancel-booking`,
    },
    { apiName: this.apiName,...config });
  

  completeBookingByIdAndCancellationToken = (id: string, cancellationToken: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/booking/${id}/complete-booking`,
    },
    { apiName: this.apiName,...config });
  

  confirmBookingByIdAndCancellationToken = (id: string, cancellationToken: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/booking/${id}/confirm-booking`,
    },
    { apiName: this.apiName,...config });
  

  getBookingByIdAndCancellationToken = (id: string, cancellationToken: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BookingResponse>({
      method: 'GET',
      url: `/api/app/booking/${id}/booking`,
    },
    { apiName: this.apiName,...config });
  

  rejectBookingByIdAndCancellationToken = (id: string, cancellationToken: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/booking/${id}/reject-booking`,
    },
    { apiName: this.apiName,...config });
  

  reserveBookingByRequestAndCancellationToken = (request: ReserveBookingDto, cancellationToken: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
      url: '/api/app/booking/reserve-booking',
      body: request,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
