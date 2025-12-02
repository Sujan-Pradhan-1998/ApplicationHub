import { describe, it, expect, vi } from 'vitest';
import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import Crud from '../../../components/crud/Index';

vi.mock('../../../components/crud/List', () => ({
    default: (props: any) => (
        <div>
            List Component
            {props.editButton && <span>Edit Enabled</span>}
            {props.deleteButton && <span>Delete Enabled</span>}
            {props.addButton && <span>Add Enabled</span>}
        </div>
    ),
}));
vi.mock('../../../components/crud/Edit', () => ({
    Edit: ({ DataForm }: any) => <div><DataForm /></div>,
}));

const MockForm: any = () => <div>Form Component</div>;

vi.mock('react-router-dom', async () => {
    const actual = await vi.importActual<any>('react-router-dom');
    return {
        ...actual,
        useNavigate: () => vi.fn(),
    };
});

describe('Crud Component', () => {
    it('renders list route with correct buttons and title', () => {
        render(
            <MemoryRouter initialEntries={['/']}>
                <Crud
                    basePath="/"
                    apiBaseEndPoint="/api/test"
                    DataFormComponent={MockForm}
                    columns={[]}
                    editButton={true}
                    deleteButton={true}
                    addButton={true}
                    filterColumns={[]}
                    title="Test CRUD"
                    modelInit={vi.fn()}
                />
            </MemoryRouter>
        );

        expect(screen.getByText('Test CRUD')).toBeDefined();
        expect(screen.getByText('List Component')).toBeDefined();
        expect(screen.getByText('Edit Enabled')).toBeDefined();
        expect(screen.getByText('Delete Enabled')).toBeDefined();
        expect(screen.getByText('Add Enabled')).toBeDefined();
    });

    it('renders entry route correctly', () => {
        render(
            <MemoryRouter initialEntries={['/entry/1']}>
                <Crud
                    basePath="/"
                    apiBaseEndPoint="/api/test"
                    DataFormComponent={MockForm}
                    columns={[]}
                    editButton={true}
                    deleteButton={true}
                    addButton={true}
                    filterColumns={[]}
                    title="Test CRUD"
                    modelInit={vi.fn()}
                />
            </MemoryRouter>
        );

        expect(screen.getByText('Test CRUD')).toBeDefined();
        expect(screen.getByText('Form Component')).toBeDefined();
    });
});
