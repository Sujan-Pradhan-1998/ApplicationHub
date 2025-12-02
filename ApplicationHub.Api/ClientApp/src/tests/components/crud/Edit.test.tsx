import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import { Edit } from '../../../components/crud/Edit';
import { describe, expect, it, vi, beforeEach } from 'vitest';
import * as ajaxService from '../../../services/ajaxService';

vi.mock('../../../services/ajaxService', () => ({
  get: vi.fn(),
}));

describe('Edit Component', () => {
  const mockModelInit = vi.fn();
  const mockInitEntry = vi.fn();
  const mockDataForm = <div>Form Content</div>;

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders DataForm inside Card', () => {
    render(
      <Edit
        id={null}
        url="/fake-url"
        modelInit={mockModelInit}
        initEntry={mockInitEntry}
        DataForm={mockDataForm}
        title=""
        gotoView={() => {}}
      />
    );
    expect(screen.getByText('Form Content')).toBeDefined();
  });

  it('calls initEntry when id is null', () => {
    render(
      <Edit
        id={null}
        url="/fake-url"
        modelInit={mockModelInit}
        initEntry={mockInitEntry}
        DataForm={mockDataForm}
        title=""
        gotoView={() => {}}
      />
    );
    expect(mockInitEntry).toHaveBeenCalled();
  });

  it('fetches item and calls modelInit when id is provided', async () => {
    const fakeData = { Id: 123, name: 'Test' };
    (ajaxService.get as any).mockResolvedValue({ data: fakeData });

    render(
      <Edit
        id={123}
        url="/fake-url"
        modelInit={mockModelInit}
        initEntry={mockInitEntry}
        DataForm={mockDataForm}
        title=""
        gotoView={() => {}}
      />
    );

    await waitFor(() => {
      expect(ajaxService.get).toHaveBeenCalledWith('/fake-url/123');
      expect(mockModelInit).toHaveBeenCalledWith(fakeData);
    });
  });

  it('assigns id to model if Id is missing', async () => {
    const fakeData = { name: 'NoId' };
    (ajaxService.get as any).mockResolvedValue({ data: fakeData });

    render(
      <Edit
        id={999}
        url="/fake-url"
        modelInit={mockModelInit}
        initEntry={mockInitEntry}
        DataForm={mockDataForm}
        title=""
        gotoView={() => {}}
      />
    );

    await waitFor(() => {
      expect(mockModelInit).toHaveBeenCalledWith({ ...fakeData, Id: 999 });
    });
  });
});
