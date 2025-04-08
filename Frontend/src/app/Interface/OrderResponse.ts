import { ProductVariantResponse } from './ProductVariantResponse';

export interface OrderResponse {
  id: string;
  createdDate: string;
  paymentMethod: number;
  statusProduct: number;
  shippingAddress: string;
  recipientName: string;
  phoneNumber: string;
  productVariantResponse: ProductVariantResponse[];
  totalAmount: number;
}
