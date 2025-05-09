import { Box, Button, Container, TextField, Typography } from '@mui/material'
import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { login } from '../../services/authService'

const LoginPage = () => {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const navigate = useNavigate()

  const handleLogin = async () => {
    const result = await login({ email, password })
    if (result) {
      localStorage.setItem('token', result.token)
      navigate('/movies')
    }
  }

  const handleGuestAccess = (e: any) => {
    e.preventDefault()
    navigate('/movies')
  }

  return (
    <Container maxWidth='xs'>
      <Box
        mt={4}
        mb={2}
        display='flex'
        justifyContent='space-between'
        alignItems='center'
      >
        <Typography variant='h4'>Login</Typography>
        <Button
          variant='text'
          size='small'
          onClick={(e) => handleGuestAccess(e)}
        >
          Continue as Guest
        </Button>
      </Box>

      <TextField
        label='Email'
        fullWidth
        margin='normal'
        value={email}
        onChange={(e) => setEmail(e.target.value)}
      />
      <TextField
        label='Password'
        type='password'
        fullWidth
        margin='normal'
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <Button variant='contained' fullWidth onClick={handleLogin}>
        Login
      </Button>

      <Typography variant='body2' align='center' mt={2}>
        Don't have an account? <Link to='/register'>Register here</Link>
      </Typography>
    </Container>
  )
}

export default LoginPage
