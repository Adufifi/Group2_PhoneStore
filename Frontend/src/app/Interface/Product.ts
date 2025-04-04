export interface Product {
  id: string;
  productName: string;
  brandName: string;
  brandId: string;
  price: number;
  image: string | null;
  description: string;
  isPromoted: boolean;
  buyCount: number;
  createdDate: Date;
}
