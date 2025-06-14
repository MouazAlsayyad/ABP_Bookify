
export interface SearchApartmentDto {
  startDate?: string;
  endDate?: string;
  page: number;
  pageSize: number;
  searchKey?: string;
}
