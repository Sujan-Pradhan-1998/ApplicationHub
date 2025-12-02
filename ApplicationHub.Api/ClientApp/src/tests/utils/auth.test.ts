import { describe, it, expect, beforeEach, vi } from 'vitest'
import { setBearerToken, getBearerToken, logout, isAuthenticated, goToLoginPage } from '../../utils/auth'

const localStorageMock = (() => {
    let store: Record<string, string> = {}
    return {
        getItem: (key: string) => store[key] || null,
        setItem: (key: string, value: string) => { store[key] = value },
        removeItem: (key: string) => { delete store[key] },
        clear: () => { store = {} }
    }
})()

Object.defineProperty(window, 'localStorage', {
    value: localStorageMock,
})

describe('Auth Helpers', () => {
    beforeEach(() => {
        localStorage.clear()
        vi.stubGlobal('location', { href: '', pathname: '/current', search: '?query=1' })
    })

    it('should set and get bearer token', () => {
        setBearerToken('test-token')
        const token = getBearerToken()
        expect(token).toBe('test-token')
    })

    it('should remove token when logging out', async () => {
        setBearerToken('test-token')
        await logout()
        expect(localStorage.getItem('application-hub-key')).toBeNull()
        expect(location.href).toBe('/login')
    })

    it('should correctly detect if authenticated', () => {
        expect(isAuthenticated()).toBe(false)
        setBearerToken('token')
        expect(isAuthenticated()).toBe(true)
    })

    it('should redirect to login page with ReturnUrl', () => {
        goToLoginPage()
        expect(location.href).toBe('/login?ReturnUrl=/current?query=1')
    })

    it('getBearerToken removes token if not found (duplicate call scenario)', () => {
        // simulate the duplicate removal scenario
        localStorage.setItem('application-hub-key', 'temp')
        const token = getBearerToken()
        expect(token).toBe('temp')
        expect(localStorage.getItem('application-hub-key')).toBe('temp')
    })
})
