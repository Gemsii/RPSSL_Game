import { RouterProvider, useRouter } from './router/RouterContext';
import { PlayerProvider } from './context/PlayerContext';
import LoginPage from './pages/LoginPage';
import GamePage from './pages/GamePage';

function AppRoutes() {
  const { currentRoute } = useRouter();

  switch (currentRoute) {
    case '/game':
      return <GamePage />;
    case '/':
    default:
      return <LoginPage />;
  }
}

function App() {
  return (
    <PlayerProvider>
      <RouterProvider>
        <AppRoutes />
      </RouterProvider>
    </PlayerProvider>
  );
}

export default App;
