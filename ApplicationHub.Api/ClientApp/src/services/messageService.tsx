import React from 'react'
import { message } from 'antd';

export const showSuccessMessage = (title?: string | React.ReactNode) => {
    message.destroy();
    message.success(title || "Task completed.")
}

export const showErrorMessage = (title?: string | React.ReactNode) => {
    message.destroy();
    message.error(title || "Task failed, please retry.", 5)
}

export const showWarningMessage = (title: string | React.ReactNode) => {
    message.destroy();
    message.warning(title)
}

export const showLoadingMessage = (title?: string | React.ReactNode) => {
    message.destroy();
    message.loading(title || "Please wait ...", 0)
}
