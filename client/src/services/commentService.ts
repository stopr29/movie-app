import toast from 'react-hot-toast'
import { API_BASE_URL } from '../constants'
import { fetchMovieDetails } from './movieService'

export const addComment = async (
  movieId: number,
  content: string,
  token: string
) => {
  try {
    const res = await fetch(`${API_BASE_URL}/comments`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({ movieId, content }),
    })

    if (!res.ok) {
      throw new Error(res.statusText)
    }

    return await fetchMovieDetails(movieId)
  } catch (err: any) {
    toast.error(err.message)
    return null
  }
}
