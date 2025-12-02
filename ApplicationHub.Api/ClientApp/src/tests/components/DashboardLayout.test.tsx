import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import DashboardLayout from '../../components/DashboardLayout';
import * as ajaxService from '../../services/ajaxService';

vi.mock('../../services/ajaxService', () => ({
  get: vi.fn(),
  post: vi.fn(),
}));

vi.mock('../../utils/auth', () => ({
  logout: vi.fn(),
}));

describe('DashboardLayout', () => {
  it('renders user name from API', async () => {
    (ajaxService.get as any).mockResolvedValue({
      data: { currentCompany: 'TestCo', email: 'test@example.com', id: '1', userName: 'John Doe' }
    });

    render(
      <MemoryRouter>
        <DashboardLayout />
      </MemoryRouter>
    );

    const userName = await screen.findByText('John Doe');
    expect(userName).toBeDefined();
  });

  it('toggles sidebar collapse', async () => {
    render(
      <MemoryRouter>
        <DashboardLayout />
      </MemoryRouter>
    );

    const toggleBtn = screen.getByRole('button', { name: /menu/i });
    fireEvent.click(toggleBtn);
    fireEvent.click(toggleBtn);
  });

  it('calls logout on button click', async () => {
    render(
      <MemoryRouter>
        <DashboardLayout />
      </MemoryRouter>
    );

    const logoutBtn = screen.getByText('Logout');
    fireEvent.click(logoutBtn);

    expect(ajaxService.post).toHaveBeenCalledWith('login/logout', {});
  });
});
