import { describe, it, expect, vi } from "vitest";
import { render } from "@testing-library/react";
import { MemoryRouter, Routes, Route } from "react-router-dom";
import ProtectedRoute from "../../components/ProtectedRoute";
import * as auth from "../../utils/auth";

describe("ProtectedRoute", () => {
  it("redirects to /login if no token", () => {
    vi.spyOn(auth, "getBearerToken").mockReturnValue(null);

    const { getByText } = render(
      <MemoryRouter initialEntries={["/protected"]}>
        <Routes>
          <Route element={<ProtectedRoute />}>
            <Route path="/protected" element={<div>Protected</div>} />
          </Route>
          <Route path="/login" element={<div>Login Page</div>} />
        </Routes>
      </MemoryRouter>
    );

    expect(getByText("Login Page")).toBeDefined();
  });

  it("renders child routes if token exists", () => {
    vi.spyOn(auth, "getBearerToken").mockReturnValue("token123");

    const { getByText } = render(
      <MemoryRouter initialEntries={["/protected"]}>
        <Routes>
          <Route element={<ProtectedRoute />}>
            <Route path="/protected" element={<div>Protected</div>} />
          </Route>
          <Route path="/login" element={<div>Login Page</div>} />
        </Routes>
      </MemoryRouter>
    );

    expect(getByText("Protected")).toBeDefined();
  });
});
