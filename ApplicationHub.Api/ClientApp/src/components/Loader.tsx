import { Spin } from "antd";
import { LoadingOutlined } from '@ant-design/icons'
import type { LoaderProps } from '../models/LoaderModel';


function getSpinner(props: LoaderProps) {
    const loadingText = props.tip || 'Loading...'
    const commonProps: LoaderProps = { ...props, tip: loadingText }
    switch (props.type) {
        case 'brand':
            return <Spin {...commonProps}>
                {props.children}
            </Spin>
        case 'circle':
            const antIcon = <LoadingOutlined type="loading" style={{ fontSize: 24 }} spin />;
            return <Spin {...commonProps} indicator={antIcon}>
                {props.children}
            </Spin>
        default:
            return <Spin {...commonProps} indicator={<LoadingOutlined type="loading" style={{ fontSize: 24 }} spin />}>
                {props.children}
            </Spin>

    }
}

export const Loader = (props: LoaderProps) => {
    const className = props.children ? '' : 'flex-center'
    return <div className={className} style={{ minHeight: '4rem' }}>
        {getSpinner(props)}
    </div>

}