export interface ProductVariant {
  id: string;
  productId: string;
  productName?: string;
  colorId: string;
  colorName?: string;
  capacityId: string;
  capacityName?: string;
  image?: string;
  quantity: number;
  price: number;
  createdDate?: Date;
}
