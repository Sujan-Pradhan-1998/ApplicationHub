import { describe, it, beforeAll, vi, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import ApplicationFormPage from '../../pages/application-form/ApplicationForm';
import * as ajaxService from '../../services/ajaxService';

vi.mock('../../services/ajaxService');
vi.mock('../../services/messageService');

beforeAll(() => {
  Object.defineProperty(window, 'matchMedia', {
    writable: true,
    value: (query: string) => ({
      matches: false,
      media: query,
      onchange: null,
      addListener: vi.fn(),
      removeListener: vi.fn(),
      addEventListener: vi.fn(),
      removeEventListener: vi.fn(),
      dispatchEvent: vi.fn(),
    }),
  });
});

describe('ApplicationFormPage', () => {
  it('renders form fields', async () => {
    (ajaxService.get as any).mockResolvedValue({ data: ['Pending', 'Approved'] });

    render(
      <MemoryRouter>
        <ApplicationFormPage />
      </MemoryRouter>
    );

    expect(screen.findByLabelText("Applied On")).toBeDefined();
    expect(screen.findByLabelText("Company")).toBeDefined();
    expect(screen.findByLabelText("Position")).toBeDefined();
    expect(screen.findByLabelText("Form Status")).toBeDefined();
    expect(screen.findByLabelText("Job Description")).toBeDefined();
  });
});
