export interface EditProps {
    title: string;
    onReset?: () => void;
    gotoView: () => void;
    liveTitle?: string;
    url: string;
    DataForm: React.ReactNode;
    id?: any;
    itemGetUrl?: string;
    modelInit: (model: any) => void;
    initEntry?: () => void;
}

export interface DataFormProps<T = number> {
    id: T;
    goToList: () => void;
}

export interface Entity {
    [index: string]: any;
    id: number;
}

export interface CustomButton<T> {
    icon: string | React.ReactNode,
    className?: string,
    onClick: (record: T) => void
    renderer?: (record: T) => boolean
    label: string
}

export interface Column<T> {
    title: string | React.ReactNode
    key: keyof T
    dataIndex: keyof T
    render?: (text: any, record: T, index: number) => React.ReactNode
    onCell?: (record: T, rowIndex: number) => any
    align?: 'left' | 'right' | 'center'
    className?: string
}

export interface CrudIndexProps<T> {
    DataFormComponent: React.ReactNode;
    customActions?: CustomButton<T>[]
    title: string;
    basePath: string;
    apiBaseEndPoint: string;
    itemGetUrl?: string
    showList?: boolean
    filterColumns?: string[]
    columns: Column<T>[];
    editButton?: boolean;
    deleteButton?: boolean;
    addButton?: boolean;
    modelInit: (model: any) => void
    gotoPath?: (url: string) => void
    onEdit?: (record: any) => void
    onAdd?: () => void
    initEntry?: () => void
    rowClassName?: (rec: T) => string
    headerButtons?: CustomButton<any>[]
}

export type CrudProps<T> = CrudIndexProps<T>;


export interface PageData<T> {
    pageNumber: number;
    totalPages: number;
    totalRecords: number;
    pageSize: number;
    items: T[];
}

export interface PageFormModel {
    pageNumber: number;
    search?: string
    searchBy?: string
    query: string
    sortDesc?: boolean
}

export interface CrudListProps {
    apiEndPoint: string;
    columns: any[];
    showList?: boolean;
    filterColumns?: string[];
    basePath: string;
    editButton?: boolean;
    onEdit?: (record: any) => void;
    deleteButton?: boolean;
    addButton?: boolean;
    onAdd?: () => void;
    customActions?: CustomButton<any>[];
    rowClassName?: (rec: any) => string;
    headerButtons?: CustomButton<any>[];
}

export interface CrudPagerProps {
    totalPages: number;
    currentPage: number;
    filterColumns?: string[];
    onPageChange: (currentPage: number, query: string, searchBy?: string) => void;
    headerButtons?: CustomButton<any>[];
}