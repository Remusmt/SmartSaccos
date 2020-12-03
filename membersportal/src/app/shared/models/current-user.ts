export interface CurrentUser {
  id: number;
  email: string;
  fullName: string;
  memberNumber: string;
  phoneNumber: string;
  tokenString: string;
  companyId: number;
  companyName: string;
  succeeded: boolean;
  status: number;
  weKnowCustomer: boolean;
}
