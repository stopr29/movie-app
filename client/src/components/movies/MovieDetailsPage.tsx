import {
  Avatar,
  Box,
  Button,
  Card,
  CardContent,
  CardMedia,
  Container,
  Divider,
  Grid,
  TextField,
  Typography,
} from '@mui/material'
import { Fragment, useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { addComment } from '../../services/commentService'
import { fetchMovieDetails } from '../../services/movieService'
import { MovieDetailsDto } from '../../types/movie'

const MovieDetailsPage = () => {
  const { movieId } = useParams()
  const [movie, setMovie] = useState<MovieDetailsDto | null>(null)

  const [newComment, setNewComment] = useState('')
  const [posting, setPosting] = useState(false)

  const token = localStorage.getItem('token')
  const isLoggedIn = Boolean(token)

  const navigate = useNavigate()

  const handleAddComment = async () => {
    setPosting(true)

    const updated = await addComment(movie!.id, newComment, token!)
    if (updated) {
      setMovie(updated)
      setNewComment('')
    }
    setPosting(false)
  }

  useEffect(() => {
    const loadMovie = async () => {
      const data = await fetchMovieDetails(Number(movieId))
      if (data) {
        setMovie(data)
      }
    }

    if (movieId) {
      loadMovie()
    }
  }, [movieId])

  if (!movie) return <Typography p={4}>Loading movie...</Typography>

  return (
    <Container maxWidth='lg' sx={{ mt: 4 }}>
      {/* Header */}
      <Box mb={4}>
        <Typography variant='h4' gutterBottom>
          {movie.title}
        </Typography>
        <Typography variant='subtitle1' color='text.secondary'>
          ⭐ {movie.voteAverage} &nbsp;&bull;&nbsp; {movie.releaseDate}
        </Typography>
        <Typography variant='body2' mt={1}>
          {movie.genres.map((g) => g.name).join(', ')}
        </Typography>
      </Box>

      {/* Poster & Overview */}
      <Grid container spacing={4}>
        <Grid>
          <img
            src={`https://image.tmdb.org/t/p/w500${movie.posterPath}`}
            alt={movie.title}
            style={{ width: '100%', borderRadius: 8 }}
          />
        </Grid>
        <Grid>
          <Typography variant='h6' gutterBottom>
            Overview
          </Typography>
          <Typography>{movie.overview}</Typography>
        </Grid>
      </Grid>

      {/* Image Gallery */}
      {movie.imageGallery.length > 0 && (
        <>
          <Typography variant='h6' mt={6} mb={2}>
            Gallery
          </Typography>
          <Grid container spacing={2}>
            {movie.imageGallery.map((url, idx) => (
              <Fragment key={url}>
                <Grid>
                  <img
                    src={`https://image.tmdb.org/t/p/w500${url}`}
                    alt={`gallery-${idx}`}
                    style={{ width: '100%', borderRadius: 4 }}
                  />
                </Grid>
              </Fragment>
            ))}
          </Grid>
        </>
      )}

      {/* Cast */}
      {movie.cast.length > 0 && (
        <>
          <Typography variant='h6' mt={6} mb={2}>
            Cast
          </Typography>
          <Grid container spacing={2}>
            {movie.cast.map((actor, idx) => (
              <Fragment key={actor.name}>
                <Grid>
                  <Card>
                    <CardMedia
                      component='img'
                      height='240'
                      image={
                        actor.profilePath
                          ? `https://image.tmdb.org/t/p/w300${actor.profilePath}`
                          : '/placeholder.jpg'
                      }
                      alt={actor.name}
                    />
                    <CardContent>
                      <Typography variant='subtitle1'>{actor.name}</Typography>
                      <Typography variant='body2' color='text.secondary'>
                        as {actor.character}
                      </Typography>
                    </CardContent>
                  </Card>
                </Grid>
              </Fragment>
            ))}
          </Grid>
        </>
      )}

      {/* Comments */}
      <Typography variant='h6' mt={6} mb={2}>
        Comments
      </Typography>
      <Box>
        {movie.comments.length === 0 ? (
          <Typography>No comments yet.</Typography>
        ) : (
          movie.comments.map((comment) => (
            <Box key={comment.id} mb={2}>
              <Box display='flex' alignItems='center' mb={0.5}>
                <Avatar sx={{ width: 28, height: 28, mr: 1 }}>
                  {comment.user.username[0]}
                </Avatar>
                <Typography variant='subtitle2'>
                  {comment.user.username}
                </Typography>
                <Typography variant='caption' color='text.secondary' ml={1}>
                  {new Date(comment.createdAt).toLocaleString()}
                </Typography>
              </Box>
              <Typography variant='body2'>{comment.content}</Typography>
              <Divider sx={{ mt: 1 }} />
            </Box>
          ))
        )}

        {isLoggedIn ? (
          <Box mt={3}>
            <Typography variant='subtitle1' mb={1}>
              Add a comment
            </Typography>
            <TextField
              fullWidth
              multiline
              minRows={2}
              label='Your comment'
              value={newComment}
              onChange={(e) => setNewComment(e.target.value)}
            />
            <Box mt={1} display='flex' justifyContent='flex-end'>
              <Button
                variant='contained'
                onClick={handleAddComment}
                disabled={!newComment.trim() || posting}
              >
                Post Comment
              </Button>
            </Box>
          </Box>
        ) : (
          <Box mt={3}>
            <Button variant='outlined' onClick={() => navigate('/login')}>
              Log in to add a comment
            </Button>
          </Box>
        )}
      </Box>
    </Container>
  )
}

export default MovieDetailsPage
