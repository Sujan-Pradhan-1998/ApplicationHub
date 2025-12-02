import { useCallback, useEffect, useMemo, useState } from "react";
import { Table, Button, Row, Col, Divider, Tooltip, Modal } from "antd";
import { useNavigate } from "react-router-dom";
import { del, get, post } from "../../services/ajaxService";
import { showSuccessMessage } from "../../services/messageService";
import type { CustomButton } from "./Index";
import Icon, { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons'
import { Loader } from "../Loader";
import { Pager } from "./Pager";

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

interface Props {
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

const List = <TPageData,>(props: Props) => {
    const navigate = useNavigate();

    const [data, setData] = useState<PageData<TPageData>>({
        pageNumber: 1,
        items: [],
        pageSize: 10,
        totalRecords: 0,
        totalPages: 1,
    });

    const [pageFormModel, setPageFormModel] = useState<PageFormModel>({
        pageNumber: 1,
        query: "",
        sortDesc: true,
    });

    const [loading, setLoading] = useState<boolean>(true);
    const getPageData = useCallback(async () => {
        setLoading(true);
        let result;

        const params = {
            PageNumber: pageFormModel.pageNumber,
            PageSize: data.pageSize
        } as any;

        const toPascalCase = (text: string): string => {
            return text
                .split(' ')
                .map(word => word.charAt(0).toUpperCase() + word.slice(1))
                .join('');
        }

        params[`Model.${toPascalCase(pageFormModel.searchBy || '')}`] = pageFormModel.query;

        const queryString = new URLSearchParams(params as any).toString();
        const res = await get<PageData<any>>(
            `${props.apiEndPoint}/filter?${queryString}`
        );
        if (res) result = res.data;


        if (result) setData(result);
        setLoading(false);
    }, [pageFormModel, props.showList, props.apiEndPoint]);

    useEffect(() => {
        getPageData();
    }, [getPageData]);

    const handlePageChange = (nextPage: number, query?: string, searchBy?: string) => {
        setPageFormModel((prev: any) => ({
            ...prev,
            pageNumber: nextPage,
            searchBy: searchBy || "",
            search: query?.trim(),
            query: query?.trim() || "",
        }));
        setData({ ...data, pageNumber: nextPage })
    };

    const gotoEntry = () => navigate(`${props.basePath}/entry`);

    const handleDelete = (record: any) => {
        Modal.confirm({
            title: "Confirm Delete",
            content: "This item will deleted permanently. Are you sure you want to delete ?",
            onOk: async () => {
                const res = await del(
                    `${props.apiEndPoint}/${record.id}`
                );
                if (res) {
                    showSuccessMessage("Item Deleted");
                    getPageData();
                }
            },
        });
    };

    const {
        filterColumns,
        columns,
        basePath,
        customActions,
        onEdit,
        onAdd,
        rowClassName,
        headerButtons,
    } = props;

    const { totalPages, pageNumber } = data;

    const getCustomBtn = (btn: CustomButton<any>, record: any) => {
        if (btn.renderer && !btn.renderer(record)) return null;

        return (
            <>
                <Divider vertical />
                <Tooltip title={btn.label} placement="topLeft" mouseEnterDelay={0.3}>
                    <a onClick={() => btn.onClick(record)}>
                        {typeof btn.icon === "string" ? (
                            <Icon type={btn.icon} className={btn.className} />
                        ) : (
                            btn.icon
                        )}
                    </a>
                </Tooltip>
            </>
        );
    };
    const editAction = (record: any) =>
        onEdit ? onEdit(record) : navigate(`${basePath}/entry/${record.id}`);

    const actionColumn = {
        title: "",
        className: "action-row",
        render: (_text: any, record: any) => (
            <span className="action-td">
                <Tooltip title="Edit" mouseEnterDelay={0.3}>
                    <a onClick={() => editAction(record)}>
                        <EditOutlined className="color-primary" />
                    </a>
                </Tooltip>
                <Divider vertical />
                <Tooltip title="Delete" mouseEnterDelay={0.3}>
                    <a onClick={() => handleDelete(record)}>
                        <DeleteOutlined style={{ color: "red" }} />
                    </a>
                </Tooltip>
                {customActions?.map((btn) => getCustomBtn(btn, record))}
            </span>
        ),
    };

    const finalColumns = useMemo(() => [...columns, actionColumn], [columns, customActions]);

    return (
        <>
            <Row className="page-crud-grid-action" justify="space-between">
                <Col xs={24} sm={4}>
                    <Button type="primary" onClick={onAdd ?? gotoEntry}>
                        <PlusOutlined type="plus" /> Add New
                    </Button>
                </Col>

                <Pager
                    filterColumns={filterColumns}
                    headerButtons={headerButtons}
                    currentPage={pageNumber}
                    totalPages={totalPages}
                    onPageChange={handlePageChange}
                />
            </Row >

            <Loader spinning={loading}>
                <Table
                    rowClassName={rowClassName}
                    locale={{ emptyText: "No Data" }}
                    rowKey={(record: any) => record.id}
                    dataSource={data.items}
                    columns={finalColumns}
                    pagination={false}
                    size="small"
                    style={{ minHeight: "10rem" }}
                />
            </Loader>
        </>
    );
};

export default List;
