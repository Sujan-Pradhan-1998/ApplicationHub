import React, { useEffect, useCallback } from 'react';
import { Card } from 'antd';
import { get } from '../../services/ajaxService';
import type { EditProps } from '../../models/CrudModel';

const Edit: React.FC<EditProps> = ({
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
