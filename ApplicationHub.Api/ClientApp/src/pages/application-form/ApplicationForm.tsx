import dayjs from 'dayjs';
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Form, Row, Col, Button, Input, DatePicker, Divider, Select } from 'antd';
import { gutterSize } from '../../constants/constants';
import Crud from '../../components/crud/Index';
import { CloseOutlined, SaveOutlined } from '@ant-design/icons';
import TextArea from 'antd/es/input/TextArea';
import { get, post, put } from '../../services/ajaxService';
import type { ApplicationFormModel } from '../../models/ApplicationModel';
import { showErrorMessage, showSuccessMessage } from '../../services/messageService';
import type { Column } from '../../models/CrudModel';

const ApplicationFormPage = () => {
    const navigate = useNavigate();
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);

    const columns: Column<ApplicationFormModel>[] = [
        {
            key: 'appliedOn', dataIndex: 'appliedOn', title: 'AppliedOn', render: (val) => val ? val.slice(0, 10) : '-'
        },
        { key: 'company', dataIndex: 'company', title: 'Company' },
        { key: 'position', dataIndex: 'position', title: 'Position' },
        { key: 'formStatus', dataIndex: 'formStatus', title: 'Form Status' },
    ];

    const params = useParams();
    const id = params['*']?.split('/')[1];

    const onSubmit = async () => {
        const values = await form.validateFields();
        const appliedOnDate = new Date(values.appliedOn);

        if (appliedOnDate.getTime() > Date.now()) {
            showErrorMessage("Applied on cannot be in future.");
            return;
        }

        setLoading(true);

        const res = id ? await put('/applicationform', { ...values, id }) :
            await post('/applicationform', { ...values });

        if (res && res.data) {
            showSuccessMessage();
            form.resetFields();
            navigate('/application-form');
        }
        setLoading(false)
    };

    const onCancel = () => {
        setLoading(false)
        form.resetFields();
        navigate('/application-form');
    }

    const [formStatus, setFormStatus] = useState<string[]>([]);

    const getFormStatus = async () => {
        const res = await get<string[]>('applicationform/select/formstatus')
        if (res) {
            setFormStatus(res.data);
        }
    }

    useEffect(() => {
        getFormStatus()
    }, [])

    const DataForm = (
        <Form form={form} layout="vertical">
            <Row gutter={gutterSize}>
                <Col xs={24} sm={24} md={5} lg={5}>
                    <Form.Item
                        label="Applied On"
                        name="appliedOn"
                        rules={[{ required: true, message: 'Required' }]}
                        getValueProps={(value) => {
                            return { value: value ? dayjs(value, 'YYYY-MM-DD') : undefined };
                        }}
                        getValueFromEvent={(date) => {
                            return date ? date.format('YYYY-MM-DD') : undefined;
                        }}
                    >
                        <DatePicker style={{ width: '100%' }} />
                    </Form.Item>
                </Col>
                <Col xs={24} sm={24} md={5} lg={5}>
                    <Form.Item label="Company" name="company" rules={[{ required: true, message: ('Required') }]}>
                        <Input style={{ width: '100%' }} />
                    </Form.Item>
                </Col>
                <Col xs={24} sm={24} md={5} lg={5}>
                    <Form.Item label="Position" name="position" rules={[{ required: true, message: ('Required') }]}>
                        <Input style={{ width: '100%' }} />
                    </Form.Item>
                </Col>
                <Col xs={24} sm={24} md={5} lg={5}>
                    <Form.Item label="Form Status" name="formStatus" rules={[{ required: true, message: ('Required') }]}>
                        <Select showSearch style={{ width: '100%' }}>
                            {formStatus.map(x => <Select.Option key={x}>{x}</Select.Option>)}
                        </Select>
                    </Form.Item>
                </Col>
            </Row>
            <Row gutter={gutterSize}>
                <Col xs={24} sm={24} md={20} lg={20}>
                    <Form.Item label="Job Description" name="description" rules={[{ required: true, message: ('Required') }]}>
                        <TextArea rows={4} style={{ width: '100%' }} />
                    </Form.Item>
                </Col>
            </Row>
            <Button icon={<SaveOutlined />} type="primary" onClick={onSubmit} loading={loading}>
                Save
            </Button>
            <Divider vertical />
            <Button icon={<CloseOutlined />} type="default" onClick={onCancel} disabled={loading}>
                Cancel
            </Button>
        </Form>
    );

    return (
        <>
            <Crud
                DataFormComponent={DataForm}
                apiBaseEndPoint="/applicationform"
                basePath="/application-form"
                columns={columns}
                deleteButton={false}
                filterColumns={['Applied On', 'Position', 'Form Status', 'Company']}
                modelInit={model => {
                    form.setFieldsValue({ ...model });
                }}
                title="Applications"
            />
        </>
    );
};

export default ApplicationFormPage;