import axios from 'axios';
import { showErrorMessage } from './messageService';
import { getBearerToken, goToLoginPage } from '../utils/auth';

export const baseEndpoint = 'http://localhost:5219/api';
const instance = axios.create({
    baseURL: baseEndpoint
})

//Add bearer token to each request if available
instance.interceptors.request.use(function (config) {
    const token = getBearerToken();
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
}, function (err) {
    return Promise.reject(err);
});

//redirect to login of 401
instance.interceptors.response.use(function (response) {
    return response;
}, function (error) {
    console.log(error)
    if (401 === error.response.status) {
        goToLoginPage()
    } else {
        return Promise.reject(error);
    }
});

function handleError(error: any, onError?: false | (() => void)) {
    if (error.response == null) {
        localStorage.clear();
        window.location.reload();
    }

    const statusCode = error.response.status || 500;

    var errorTitle = "Task failed, please retry.";
    if (error.response.data && error.response.data.ExceptionMessage)
        errorTitle = `Failed. ${error.response.data.ExceptionMessage}`;

    if (statusCode === 404) {
        errorTitle = "Content or Resource not found";
    }

    if (statusCode === 400) {
        errorTitle = "Invalid request";
    }

    if (statusCode === 401) {
        errorTitle = "Unauthorized, please login again.";
    }

    showErrorMessage(errorTitle);
    if (onError) onError();
}

export async function get<TResponse>(url: string) {
    return instance.get<TResponse>(url)
        .catch(error => {
            handleError(error)
            throw error;
        });
}

export async function post<TResponse>(url: string, body: {}, onError?: false | (() => void)) {
    try {
        const res = await instance.post<TResponse>(url, body)
        return res && res;
    } catch (error) {
        handleError(error, onError);
    }
}

export async function del(url: string) {
    try {
        return await instance.delete(url)
    } catch (error) {
        console.log(error)
        showErrorMessage("Could not delete this item");
    }
}