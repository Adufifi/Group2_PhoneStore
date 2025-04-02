export interface Customer {
  id: number;
  username: string;
  email: string;
  password: string;
  role_id: number;
  created_date: Date;
  updated_date: Date;
  status: boolean;
}