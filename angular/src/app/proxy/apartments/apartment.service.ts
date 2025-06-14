import type { ApartmentCreateUpdateDto } from './create-apartment/models';
import type { ApartmentResponse, SearchApartmentResponse } from './models';
import type { SearchApartmentDto } from './search-apartments/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ApartmentService {
  apiName = 'Default';
  

  createApartmentByRequestAndCancellationToken = (request: ApartmentCreateUpdateDto, cancellationToken: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
      url: '/api/app/apartment/apartment',
      body: request,
    },
    { apiName: this.apiName,...config });
  

  getApartmentByIdAndCancellationToken = (id: string, cancellationToken: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApartmentResponse>({
      method: 'GET',
      url: `/api/app/apartment/${id}/apartment`,
    },
    { apiName: this.apiName,...config });
  

  searchApartmentsByInputAndCancellationToken = (input: SearchApartmentDto, cancellationToken?: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SearchApartmentResponse[]>({
      method: 'POST',
      url: '/api/app/apartment/search-apartments',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  updateApartmentByIdAndRequestAndCancellationToken = (id: string, request: ApartmentCreateUpdateDto, cancellationToken: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'PUT',
      responseType: 'text',
      url: `/api/app/apartment/${id}/apartment`,
      body: request,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
