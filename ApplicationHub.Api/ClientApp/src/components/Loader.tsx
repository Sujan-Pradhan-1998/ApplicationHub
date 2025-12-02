import * as React from 'react'
import { Spin, type SpinProps } from "antd";
import { LoadingOutlined } from '@ant-design/icons'

interface Props extends SpinProps {
    children?: React.ReactNode
    type?: LoaderTypes
}
type LoaderTypes = 'brand' | 'circle' | 'default'

function getSpinner(props: Props) {
    const loadingText = props.tip || 'Loading...'
    const commonProps: Props = { ...props, tip: loadingText }
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

export const Loader = (props: Props) => {
    const className = props.children ? '' : 'flex-center'
    return <div className={className} style={{ minHeight: '4rem' }}>
        {getSpinner(props)}
    </div>

}