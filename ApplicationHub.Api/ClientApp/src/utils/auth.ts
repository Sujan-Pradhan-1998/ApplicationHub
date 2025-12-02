const localStorageAuthTokenKey = 'application-hub-key'

export function getBearerToken() {
    var token = localStorage.getItem(localStorageAuthTokenKey)
    if(!token) {
        token = localStorage.getItem(localStorageAuthTokenKey)
        localStorage.removeItem(localStorageAuthTokenKey);
    }

    return token;
}

export function setBearerToken(token: string) {
    localStorage.setItem(localStorageAuthTokenKey, token)
}

export async function logout() {
    localStorage.removeItem(localStorageAuthTokenKey);
    location.href='/login'
}

export function goToLoginPage() {
    var url = location.pathname 
    url += location.search || '';

    location.href = `/login?ReturnUrl=${url}`
}

export const isAuthenticated = () => !!getBearerToken();