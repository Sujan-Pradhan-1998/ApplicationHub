import './Index.scss'

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

interface Props<T> {
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

type CrudProps<T> = Props<T>;
import React, { useEffect, useState, useCallback } from "react";
import { Routes, Route, useNavigate, useParams } from "react-router-dom";
import List from './List';
import { Edit } from './Edit';

const Crud = <T,>(props: CrudProps<T>) => {
    const {
        basePath,
        apiBaseEndPoint,
        DataFormComponent,
        columns,
        editButton,
        deleteButton,
        addButton,
        filterColumns,
        itemGetUrl,
        customActions,
        onEdit,
        onAdd,
        initEntry,
        showList,
        rowClassName,
        headerButtons,
        title: initialTitle,
        modelInit,
    } = props;

    const [title, setTitle] = useState("");

    useEffect(() => {
        setTitle(initialTitle);
    }, [initialTitle]);

    const modelChange = useCallback(
        (model: any) => {
            const finalTitle = `${initialTitle}`;
            setTitle(finalTitle);
            modelInit(model);
        },
        [initialTitle, modelInit]
    );

    return (
        <div className="page-crud">
            <h1 className="page-crud-title">{title}</h1>

            <Routes>
                {/* List page */}
                <Route
                    path="/"
                    element={
                        <List
                            apiEndPoint={apiBaseEndPoint}
                            filterColumns={filterColumns}
                            columns={columns}
                            basePath={basePath}
                            editButton={editButton}
                            deleteButton={deleteButton}
                            showList={showList}
                            customActions={customActions}
                            onEdit={onEdit}
                            addButton={addButton}
                            headerButtons={headerButtons}
                            onAdd={onAdd}
                            rowClassName={rowClassName}
                        />
                    }
                />

                {/* Entry page */}
                <Route
                    path="entry/:id?"
                    element={
                        <CrudEntryWrapper
                            title={title}
                            DataFormComponent={DataFormComponent}
                            modelChange={modelChange}
                            apiBaseEndPoint={apiBaseEndPoint}
                            itemGetUrl={itemGetUrl}
                            initEntry={initEntry}
                            basePath={basePath}
                        />
                    }
                />
            </Routes>
        </div>
    );
};

/** Wrapper because useNavigate + useParams cannot be used inside the arrow component above */
const CrudEntryWrapper = ({
    title,
    DataFormComponent,
    modelChange,
    apiBaseEndPoint,
    itemGetUrl,
    initEntry,
    basePath
}: any) => {
    const navigate = useNavigate();
    const { id } = useParams();

    return (
        <Edit
            id={id}
            title={title}
            DataForm={DataFormComponent}
            gotoView={() => navigate(basePath)}
            modelInit={modelChange}
            url={apiBaseEndPoint}
            itemGetUrl={itemGetUrl}
            initEntry={initEntry}
        />
    );
};

export default Crud;
