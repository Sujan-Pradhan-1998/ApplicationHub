import { describe, it, expect } from "vitest";
import { render, screen } from "@testing-library/react";
import { NotFoundPage } from "../../pages/NotFound";

describe("NotFoundPage", () => {
  it("renders 404 and Page not found", () => {
    render(<NotFoundPage />);

    // Check for the 404 heading
    const heading = screen.getByRole("heading", { name: /404/i });
    expect(heading).toBeDefined();

    // Check for the paragraph text
    const paragraph = screen.getByText(/Page not found/i);
    expect(paragraph).toBeDefined();
  });
});
