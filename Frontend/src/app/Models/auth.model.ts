export interface StatusResponse {
  status: number;
  mess: string;
}

export interface AuthResultVm {
  token?: string;
  refreshToken?: string;
  status: number;
  mess: string;
}

export interface LoginVm {
  email: string;
  password: string;
}
