export interface CurrentUser {
  id: number;
  email: string;
  fullName: string;
  phoneNumber: string;
  tokenString: string;
  companyId: number;
  companyName: string;
  succeeded: boolean;
  canManageUsers: boolean;
}
