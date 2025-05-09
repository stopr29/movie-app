import { toast } from 'react-hot-toast'
import { API_BASE_URL } from '../constants'
import { AuthResponse, LoginRequest, RegisterRequest } from '../types/auth'

export const login = async (
  request: LoginRequest
): Promise<AuthResponse | null> => {
  try {
    const response = await fetch(`${API_BASE_URL}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(request),
    })

    if (!response.ok) {
      throw new Error(response.statusText)
    }

    return await response.json()
  } catch (error: any) {
    toast.error(error.message)
    return null
  }
}

export const register = async (
  request: RegisterRequest
): Promise<AuthResponse | null> => {
  try {
    const response = await fetch(`${API_BASE_URL}/auth/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(request),
    })

    if (!response.ok) {
      throw new Error(response.statusText)
    }

    return await response.json()
  } catch (error: any) {
    toast.error(error.message)
    return null
  }
}
