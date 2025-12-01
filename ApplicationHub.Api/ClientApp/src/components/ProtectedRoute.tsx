import { Navigate, Outlet } from "react-router-dom";
import { getBearerToken } from "../utils/auth";

const ProtectedRoute = () => {
  const token = getBearerToken();

  if (!token) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
