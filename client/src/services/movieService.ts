import toast from 'react-hot-toast'
import { API_BASE_URL } from '../constants'
import { MovieDetailsDto, MovieDto, SearchMoviesQuery } from '../types/movie'

export const fetchTopRatedMovies = async (): Promise<MovieDto[]> => {
  try {
    const res = await fetch(`${API_BASE_URL}/movies/top-rated`)

    if (!res.ok) {
      throw new Error(res.statusText)
    }

    return await res.json()
  } catch (err: any) {
    toast.error(err.message)
    return []
  }
}

export const searchMovies = async (
  filters: SearchMoviesQuery
): Promise<MovieDto[]> => {
  try {
    const params = new URLSearchParams()
    if (filters.query) params.append('query', filters.query)
    if (filters.genreId !== undefined)
      params.append('genreId', filters.genreId.toString())

    const res = await fetch(
      `${API_BASE_URL}/movies/search?${params.toString()}`
    )
    if (!res.ok) {
      throw new Error(res.statusText)
    }

    return await res.json()
  } catch (err: any) {
    toast.error(err.message)
    return []
  }
}

export const fetchMovieDetails = async (
  movieId: number
): Promise<MovieDetailsDto | null> => {
  try {
    const res = await fetch(`${API_BASE_URL}/movies/${movieId}`)
    if (!res.ok) {
      throw new Error(res.statusText)
    }

    return await res.json()
  } catch (err: any) {
    toast.error(err.message)
    return null
  }
}
