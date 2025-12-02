import type { SpinProps } from "antd"

export interface LoaderProps extends SpinProps {
    children?: React.ReactNode
    type?: LoaderTypes
}
export type LoaderTypes = 'brand' | 'circle' | 'default'

export interface LoginPayload {
  email: string;
  password: string;
}