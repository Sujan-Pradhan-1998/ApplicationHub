import { useState, useMemo, useCallback } from 'react';
import { Button, Input, Col, InputNumber, Dropdown, Tooltip, Select, Row, Space } from 'antd';
import type { CustomButton } from './Index';
import _ from 'lodash';

interface Props {
    totalPages: number;
    currentPage: number;
    filterColumns?: string[];
    onPageChange: (currentPage: number, query: string, searchBy?: string) => void;
    headerButtons?: CustomButton<any>[];
}

export const Pager = ({ totalPages, currentPage, filterColumns, onPageChange, headerButtons }: Props) => {
    const [query, setQuery] = useState('');
    const [searchBy, setSearchBy] = useState<string | undefined>(filterColumns && filterColumns[0]);
    const [showMore, setShowMore] = useState(false);

    const debouncedSearch = useMemo(() => _.debounce((val: string) => {
        onPageChange(currentPage, val, searchBy);
    }, 500), [currentPage, searchBy, onPageChange]);

    const prevPage = useCallback(() => {
        if (currentPage > 1) {
            onPageChange(currentPage - 1, query, searchBy);
        }
    }, [currentPage, query, searchBy, onPageChange]);

    const nextPage = useCallback(() => {
        if (currentPage < totalPages) {
            onPageChange(currentPage + 1, query, searchBy);
        }
    }, [currentPage, totalPages, query, searchBy, onPageChange]);

    const columnMenuItems = [
        {
            key: 'done',
            label: (
                <Button size="small" type="link" onClick={() => setShowMore(false)}>
                    Done
                </Button>
            )
        },
        ...(
            (headerButtons?.map((x, i) =>
                (x.renderer === undefined)
                    ? {
                        key: `btn-${i}`,
                        label: (
                            <Button onClick={x.onClick}>
                                {(x.label as any)}
                            </Button>
                        )
                    }
                    : null
            ).filter(Boolean) as any[]) ?? []
        )
    ];

    return (
        <>
            <Col xs={24} sm={24} md={20}>
                <Space.Compact style={{ width: '100%', justifyContent: 'flex-end' }}>
                    <Row justify="end" gutter={[8, 8]} style={{ width: '100%' }}>
                        <Col xs={24} sm={6} md={7}>
                            <h4 style={{ textAlign: 'right' }}>
                                Page &nbsp;
                                <InputNumber
                                    style={{ width: '50px', textAlign: 'right' }}
                                    value={currentPage}
                                    min={1}
                                    max={totalPages}
                                    onChange={(val) => onPageChange(Number(val || 1), query, searchBy)}
                                />{' '}
                                of {totalPages} &nbsp;
                            </h4>
                        </Col>


                        <Col xs={24} sm={6} md={15}>
                            <Space.Compact style={{ justifyContent: 'flex-end', width: '100%' }}>
                                {filterColumns && (
                                    <Select
                                        value={searchBy}
                                        onChange={(val) => {
                                            setSearchBy(val);
                                            setQuery('');
                                            onPageChange(1, '', val || undefined);
                                        }}
                                        placeholder="Filter By"
                                        showSearch
                                        style={{ width: '50%' }}
                                    >
                                        {filterColumns.map((x) => (
                                            <Select.Option key={x} value={x}>
                                                {x as any}
                                            </Select.Option>
                                        ))}
                                    </Select>
                                )}
                                <Input
                                    style={{ width: '100%' }}
                                    placeholder="Filter"
                                    value={query}
                                    onChange={(e) => {
                                        setQuery(e.target.value);
                                        debouncedSearch(e.target.value);
                                    }}
                                />
                                <Button type="primary" disabled={currentPage === 1} onClick={prevPage}>
                                    ← Prev
                                </Button>

                                <Button type="primary" disabled={currentPage >= totalPages} onClick={nextPage}>
                                    Next →
                                </Button>

                                {headerButtons && (
                                    <Dropdown menu={{ items: columnMenuItems }} open={showMore}>
                                        <a className="ant-dropdown-link" onClick={() => setShowMore(!showMore)}>
                                            <Tooltip title="More">
                                                <Button type="primary">⋮</Button>
                                            </Tooltip>
                                        </a>
                                    </Dropdown>
                                )}
                            </Space.Compact>
                        </Col>
                    </Row>
                </Space.Compact>
            </Col>

        </>
    );
}
