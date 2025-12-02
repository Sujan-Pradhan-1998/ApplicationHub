import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { act, fireEvent } from '@testing-library/react';
import ReactDOM from 'react-dom/client';
import { Pager } from '../../../components/crud/Pager';

describe('Pager', () => {
  let container: HTMLDivElement;
  const mockOnPageChange = vi.fn();

  const defaultProps = {
    totalPages: 5,
    currentPage: 1,
    filterColumns: ['Name', 'Age'],
    onPageChange: mockOnPageChange,
    headerButtons: [{ label: 'Test', onClick: vi.fn() }]
  } as any;

  beforeEach(() => {
    container = document.createElement('div');
    document.body.appendChild(container);
    mockOnPageChange.mockClear();

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

  afterEach(() => {
    document.body.removeChild(container);
  });

  it('renders current page and total pages', () => {
    act(() => {
      const root = ReactDOM.createRoot(container);
      root.render(<Pager {...defaultProps} />);
    });
    expect(container.textContent).toContain('Page');
    expect(container.textContent).toContain('of 5');
  });

  it('calls onPageChange when next button is clicked', () => {
    act(() => {
      const root = ReactDOM.createRoot(container);
      root.render(<Pager {...defaultProps} />);
    });

    const nextBtn = Array.from(container.querySelectorAll('button')).find(btn =>
      btn.textContent?.includes('Next →')
    );
    act(() => nextBtn?.click());
    expect(mockOnPageChange).toHaveBeenCalledWith(2, '', 'Name');
  });

  it('calls onPageChange when prev button is clicked', () => {
    act(() => {
      const root = ReactDOM.createRoot(container);
      root.render(<Pager {...defaultProps} currentPage={2} />);
    });

    const prevBtn = Array.from(container.querySelectorAll('button')).find(btn =>
      btn.textContent?.includes('← Prev')
    );
    act(() => prevBtn?.click());
    expect(mockOnPageChange).toHaveBeenCalledWith(1, '', 'Name');
  });

  it('updates query input', () => {
    act(() => {
      const root = ReactDOM.createRoot(container);
      root.render(<Pager {...defaultProps} />);
    });

    const input = container.querySelector('input[placeholder="Filter"]') as HTMLInputElement;
    act(() => {
      fireEvent.change(input, { target: { value: 'John' } });
    });
    expect(input.value).toBe('John');
  });
});
