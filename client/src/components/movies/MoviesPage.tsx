import {
  Box,
  Button,
  Card,
  CardContent,
  CardMedia,
  Container,
  Grid,
  MenuItem,
  Select,
  TextField,
  Typography,
} from '@mui/material'
import { Fragment, useCallback, useEffect, useState } from 'react'
import { toast } from 'react-hot-toast'
import { useNavigate, useSearchParams } from 'react-router-dom'
import { fetchTopRatedMovies, searchMovies } from '../../services/movieService'
import { MovieDto, SearchMoviesQuery } from '../../types/movie'

const MoviesPage = () => {
  const [movies, setMovies] = useState<MovieDto[]>([])
  const [query, setQuery] = useState('')
  const [genreId, setGenreId] = useState<number | ''>('')
  const [searchParams, setSearchParams] = useSearchParams()
  const navigate = useNavigate()

  const loadTopRated = async () => {
    try {
      const data = await fetchTopRatedMovies()
      setMovies(data)
    } catch (error: any) {
      toast.error(error.message)
    }
  }

  const performSearch = useCallback(async () => {
    const filters: SearchMoviesQuery = {
      query,
      genreId: genreId !== '' ? genreId : undefined,
    }

    const data = await searchMovies(filters)
    setMovies(data)

    const params: Record<string, string> = {}
    if (filters.query) params.query = filters.query
    if (filters.genreId !== undefined)
      params.genreId = filters.genreId.toString()
    setSearchParams(params)
  }, [genreId, query, setSearchParams])

  useEffect(() => {
    const queryParam = searchParams.get('query') ?? ''
    const genreParam = searchParams.get('genreId')

    setQuery(queryParam)
    setGenreId(genreParam ? parseInt(genreParam) : '')

    if (queryParam || genreParam) {
      performSearch()
    } else {
      loadTopRated()
    }
  }, [performSearch, searchParams])

  const handleMovieClick = (id: number) => {
    navigate(`/movies/${id}`)
  }

  return (
    <Container maxWidth='lg'>
      <Box mt={4} mb={2} display='flex' gap={2}>
        <TextField
          label='Search movies'
          fullWidth
          value={query}
          onChange={(e) => setQuery(e.target.value)}
        />
        <Select
          displayEmpty
          value={genreId}
          onChange={(e: any) =>
            setGenreId(e.target.value === '' ? '' : parseInt(e.target.value))
          }
        >
          <MenuItem value=''>All Genres</MenuItem>
          <MenuItem value={28}>Action</MenuItem>
          <MenuItem value={35}>Comedy</MenuItem>
          <MenuItem value={18}>Drama</MenuItem>
          {/* Add more genres as needed */}
        </Select>
        <Button variant='contained' onClick={performSearch}>
          Search
        </Button>
      </Box>

      <Grid container spacing={3}>
        {movies.map((movie) => (
          <Fragment key={movie.id}>
            <Grid>
              <Card
                sx={{ cursor: 'pointer' }}
                onClick={() => handleMovieClick(movie.id)}
              >
                <CardMedia
                  component='img'
                  height='360'
                  image={
                    movie.posterPath
                      ? `https://image.tmdb.org/t/p/w500${movie.posterPath}`
                      : '/placeholder.jpg'
                  }
                  alt={movie.title}
                />
                <CardContent>
                  <Typography variant='subtitle1'>{movie.title}</Typography>
                  <Typography variant='body2' color='text.secondary'>
                    ⭐ {movie.voteAverage} &nbsp;&bull;&nbsp;{' '}
                    {movie.releaseDate}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          </Fragment>
        ))}
      </Grid>
    </Container>
  )
}
export default MoviesPage
