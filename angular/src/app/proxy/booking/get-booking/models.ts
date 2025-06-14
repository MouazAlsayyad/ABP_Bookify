
export interface BookingResponse {
  id?: string;
  userId?: string;
  apartmentId?: string;
  status: number;
  priceAmount: number;
  priceCurrency?: string;
  cleaningFeeAmount: number;
  cleaningFeeCurrency?: string;
  amenitiesUpChargeAmount: number;
  amenitiesUpChargeCurrency?: string;
  totalPriceAmount: number;
  totalPriceCurrency?: string;
  durationStart?: string;
  durationEnd?: string;
  createdOnUtc?: string;
}
