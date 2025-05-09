import { UserDto } from './user'

export interface CommentDto {
  id: string
  user: UserDto
  content: string
  createdAt: string
}
