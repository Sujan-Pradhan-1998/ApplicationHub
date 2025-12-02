export interface LoginModel {
  email: string;
  password: string
}

export interface RegisterModel {
  firstName: string;
  middleName: string;
  lastName: string;
  currentCompany: string;
  email: string;
  confirmEmail: string;
  password: string;
  confirmPassword: string;
}
export interface UserModel {
  firstName: string;
  middleName: string;
  lastName: string;
  currentCompany: string;
  email: string;
}
