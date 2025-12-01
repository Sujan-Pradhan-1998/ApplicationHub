import axios from "axios";
import { baseEndpoint } from "./ajaxService";

export interface LoginPayload {
  email: string;
  password: string;
}

export const login = async (payload: LoginPayload) => {
  const response = await axios.post(`${baseEndpoint}/login`, payload);
  return response.data;
};
