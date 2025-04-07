export interface Role {
  id: string;
  roleName: string;
}

export interface Customer {
  id: string;
  userName: string;
  email: string;
  password: string;
  role_id: string; // Guid type
  role?: Role;
  created_date: Date;
  img?: string;
  roleName?: string;
  createdDate?: Date;
}