
export interface CreateProductCommand {
  name?: string;
  price: number;
  description?: string;
}

export interface ProductDto {
  id?: string;
  name?: string;
  price: number;
  description?: string;
}
