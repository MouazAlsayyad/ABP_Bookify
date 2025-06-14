import type { Amenity } from '../amenity.enum';

export interface AddressCreateUpdateDto {
  country?: string;
  state?: string;
  zipCode?: string;
  city?: string;
  street?: string;
}

export interface ApartmentCreateUpdateDto {
  name?: string;
  description?: string;
  address: AddressCreateUpdateDto;
  priceAmount: number;
  cleaningFeeAmount: number;
  currency?: string;
  amenities?: Amenity[];
}
