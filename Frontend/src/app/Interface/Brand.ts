import { Product } from './Product';

export interface Brand {
  id: string;
  name: string;
  products: Product[];
  createdDate: string;
}
