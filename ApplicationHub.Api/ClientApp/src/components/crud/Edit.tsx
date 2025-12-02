import React, { useEffect, useCallback } from 'react';
import { Card } from 'antd';
import { get } from '../../services/ajaxService';

interface Props {
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

const Edit: React.FC<Props> = ({
    id,
    itemGetUrl,
    url,
    modelInit,
    initEntry,
    DataForm
}) => {

    const initEdit = useCallback(async (entryId: any) => {
        const fetchUrl = itemGetUrl
            ? `${itemGetUrl}/${entryId}`
            : `${url}/${entryId}`;

        const res = await get<any>(fetchUrl);
        if (res) {
            const model = res.data || {};
            if (!model.Id) {
                model.Id = entryId;
            }
            modelInit(model);
        }
    }, [itemGetUrl, url, modelInit]);

    useEffect(() => {
        if (id) {
            initEdit(id);
        } else {
            initEntry?.();
        }
    }, [id, initEdit, initEntry]);

    return (
        <Card style={{ maxWidth: '98%' }}>
            {DataForm}
        </Card>
    );
};

export { Edit };
