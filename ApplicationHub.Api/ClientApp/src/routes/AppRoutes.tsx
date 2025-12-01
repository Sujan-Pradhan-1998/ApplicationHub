import { Routes, Route } from "react-router-dom";
import DashboardLayout from "../components/DashboardLayout";
import ProtectedRoute from "../components/ProtectedRoute";
import UsersPage from "../pages/user/User";
import LoginPage from "../pages/login/Login";
import { NotFoundPage } from "../pages/NotFound";

export default function AppRouter() {
  return (
    <Routes>

      <Route path="/login" element={<LoginPage />} />

      <Route element={<ProtectedRoute />}>
        <Route path="/" element={<DashboardLayout />}>

          <Route index path="/dashboard" element={<></>} />
          <Route path="/user" element={<UsersPage />} />
          {/* <Route path="/application-form" element={<ApplicationForm />} /> */}
          <Route path="*" element={<NotFoundPage />} />
        </Route>
      </Route>
    </Routes>
  );
}
