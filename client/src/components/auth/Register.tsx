import { Box, Button, Container, TextField, Typography } from '@mui/material'
import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { register } from '../../services/authService'

const RegisterPage = () => {
  const [email, setEmail] = useState('')
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const navigate = useNavigate()

  const handleRegister = async () => {
    const result = await register({ email, username, password })
    if (result) {
      localStorage.setItem('token', result.token)
      navigate('/movies')
    }
  }

  const handleGuestAccess = () => {
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
        <Button variant='text' size='small' onClick={handleGuestAccess}>
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
        label='Username'
        fullWidth
        margin='normal'
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      />
      <TextField
        label='Password'
        type='password'
        fullWidth
        margin='normal'
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <Button variant='contained' fullWidth onClick={handleRegister}>
        Register
      </Button>

      <Typography variant='body2' align='center' mt={2}>
        Already have an account? <Link to='/login'>Login here</Link>
      </Typography>
    </Container>
  )
}

export default RegisterPage
