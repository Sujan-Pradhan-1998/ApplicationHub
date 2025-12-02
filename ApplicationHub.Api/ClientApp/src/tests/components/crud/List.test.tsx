import { describe, it, expect, vi, beforeAll } from "vitest";
import { render, screen } from "@testing-library/react";
import List from "../../../components/crud/List";

const mockProps = {
  apiEndPoint: "/api/items",
  basePath: "/items",
  columns: [{ title: "Name", dataIndex: "name", key: "name" }],
  filterColumns: [],
};

vi.mock("../../../services/ajaxService", () => ({
  get: vi.fn(() =>
    Promise.resolve({
      data: { pageNumber: 1, items: [], pageSize: 10, totalRecords: 0, totalPages: 1 },
    })
  ),
  del: vi.fn(() => Promise.resolve({})),
}));

vi.mock("../../../services/messageService", () => ({
  showSuccessMessage: vi.fn(),
}));

vi.mock("react-router-dom", () => ({
  useNavigate: () => vi.fn(),
}));

beforeAll(() => {
  Object.defineProperty(window, "matchMedia", {
    writable: true,
    value: (query: string) => ({
      matches: false,
      media: query,
      onchange: null,
      addListener: vi.fn(), // deprecated
      removeListener: vi.fn(), // deprecated
      addEventListener: vi.fn(),
      removeEventListener: vi.fn(),
      dispatchEvent: vi.fn(),
    }),
  });
});

describe("List Component", () => {
  it("renders without crashing", async () => {
    render(<List {...mockProps} />);
    expect(await screen.findByText("No Data")).toBeTruthy();
    expect(screen.getByText("Add New")).toBeTruthy();
  });
});
