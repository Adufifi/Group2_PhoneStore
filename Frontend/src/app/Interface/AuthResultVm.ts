export interface AuthResultVm {
  status: number;
  mess?: string;
  token?: string;
  refreshToken?: string;
  expireAt?: string;
}
