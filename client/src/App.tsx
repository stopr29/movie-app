import { Toaster } from 'react-hot-toast'
import {
  Navigate,
  Route,
  BrowserRouter as Router,
  Routes,
} from 'react-router-dom'
import LoginPage from './components/auth/Logix'
import RegisterPage from './components/auth/Register'
import MovieDetailsPage from './components/movies/MovieDetailsPage'
import MoviesPage from './components/movies/MoviesPage'

function App() {
  return (
    <>
      <Toaster
        position='bottom-left'
        toastOptions={{
          success: {
            style: {
              background: '#4caf50',
              color: 'white',
            },
          },
          error: {
            style: {
              background: '#f44336',
              color: 'white',
            },
          },
        }}
      />
      <Router>
        <Routes>
          <Route path='*' element={<Navigate to='/login' replace />} />
          <Route path='/login' element={<LoginPage />} />
          <Route path='/register' element={<RegisterPage />} />
          <Route path='/movies' element={<MoviesPage />} />
          <Route path='/movies/:movieId' element={<MovieDetailsPage />} />
        </Routes>
      </Router>
    </>
  )
}

export default App
