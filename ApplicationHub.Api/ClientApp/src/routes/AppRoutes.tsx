import { Routes, Route } from "react-router-dom";
import DashboardLayout from "../components/DashboardLayout";
import ProtectedRoute from "../components/ProtectedRoute";
import LoginPage from "../pages/login/Login";
import { NotFoundPage } from "../pages/NotFound";
import ApplicationFormPage from "../pages/application-form/ApplicationForm";

export default function AppRouter() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route element={<ProtectedRoute />}>
        <Route path="/" element={<DashboardLayout />}>
          <Route index path="/dashboard" element={<></>} />
          <Route path="/application-form/*" element={<ApplicationFormPage />} />
          <Route path="*" element={<NotFoundPage />} />
        </Route>
      </Route>
    </Routes>
  );
}
