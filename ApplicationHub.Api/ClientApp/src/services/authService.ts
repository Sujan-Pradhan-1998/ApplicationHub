import axios from "axios";
import { baseEndpoint } from "./ajaxService";
import type { LoginPayload } from "../models/LoaderModel";


export const login = async (payload: LoginPayload) => {
  const response = await axios.post(`${baseEndpoint}/login`, payload);
  return response.data;
};
