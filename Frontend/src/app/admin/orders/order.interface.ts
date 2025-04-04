export interface Order {
  id: string;
  customerName: string;
  orderDate: string;
  status: 'pending' | 'processing' | 'completed' | 'cancelled';
  total: number;
  phone: string;
  address: string;
}
