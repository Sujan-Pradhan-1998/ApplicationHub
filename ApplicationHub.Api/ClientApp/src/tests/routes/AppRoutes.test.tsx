import { describe, it, expect, vi } from "vitest";
import { render, screen } from "@testing-library/react";
import { MemoryRouter, Outlet } from "react-router-dom";
import AppRouter from "../../routes/AppRoutes";

// Mock ProtectedRoute to just render its children via Outlet
vi.mock("../../components/ProtectedRoute", () => ({
  default: ({ children }: any) => <>{children}</>,
}));

// Mock DashboardLayout to render children via Outlet
vi.mock("../../components/DashboardLayout", () => ({
  default: () => (
    <div>
      Dashboard Layout
      <Outlet />
    </div>
  ),
}));

// Mock pages with simple identifiable text
vi.mock("../../pages/login/Login", () => ({
  default: () => <div>Login Page</div>,
}));

vi.mock("../../pages/NotFound", () => ({
  NotFoundPage: () => <div>Not Found</div>,
}));

vi.mock("../../pages/application-form/ApplicationForm", () => ({
  default: () => <div>Application Form</div>,
}));

describe("AppRouter", () => {
  it("renders login page at /login", () => {
    render(
      <MemoryRouter initialEntries={["/login"]}>
        <AppRouter />
      </MemoryRouter>
    );
    expect(screen.findAllByText(/login page/i)).toBeDefined();
  });

  it("renders DashboardLayout at /dashboard", () => {
    render(
      <MemoryRouter initialEntries={["/dashboard"]}>
        <AppRouter />
      </MemoryRouter>
    );
    expect(screen.findAllByText(/dashboard layout/i)).toBeDefined();
  });

  it("renders ApplicationFormPage at /application-form", () => {
    render(
      <MemoryRouter initialEntries={["/application-form"]}>
        <AppRouter />
      </MemoryRouter>
    );
    expect(screen.findAllByText(/application form/i)).toBeDefined();
  });

  it("renders NotFoundPage for unknown route", () => {
    render(
      <MemoryRouter initialEntries={["/unknown-route"]}>
        <AppRouter />
      </MemoryRouter>
    );
    expect(screen.findAllByText("Page not found")).toBeDefined();
  });
});
