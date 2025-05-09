import { CommentDto } from './comment'

export interface MovieDto {
  id: number
  title: string
  posterPath: string
  voteAverage: number
  releaseDate: string
}

export interface MovieDetailsDto {
  id: number
  title: string
  overview: string
  posterPath: string
  backdropPath: string
  releaseDate: string
  voteAverage: number
  imageGallery: string[]
  cast: CastDto[]
  genres: GenreDto[]
  comments: CommentDto[]
}

export interface CastDto {
  name: string
  character: string
  profilePath: string
}

export interface GenreDto {
  id: number
  name: string
}

export interface SearchMoviesQuery {
  query: string
  genreId?: number
}
