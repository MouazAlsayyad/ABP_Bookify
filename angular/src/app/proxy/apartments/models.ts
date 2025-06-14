import type { Amenity } from './amenity.enum';

export interface AddressResponse {
  country?: string;
  state?: string;
  zipCode?: string;
  city?: string;
  street?: string;
}

export interface ApartmentResponse {
  id?: string;
  name?: string;
  description?: string;
  price: number;
  currency?: string;
  averageRating?: number;
  reviewCount: number;
  address: AddressResponse;
  amenities?: Amenity[];
  reviews: ReviewResponse[];
}

export interface ReviewResponse {
  reviewId?: string;
  userId?: string;
  firstName?: string;
  lastName?: string;
  rating: number;
  comment?: string;
  createdOnUtc?: string;
}

export interface SearchApartmentResponse {
  id?: string;
  name?: string;
  description?: string;
  price: number;
  currency?: string;
  averageRating: number;
  reviewCount: number;
  address: AddressResponse;
  amenities?: Amenity[];
}
