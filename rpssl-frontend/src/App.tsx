import { RouterProvider, useRouter } from './router/RouterContext';
import { PlayerProvider, usePlayer } from './context/PlayerContext';
import LoginPage from './pages/LoginPage';
import GamePage from './pages/GamePage';

function AppRoutes() {
  const { currentRoute } = useRouter();
  const { player } = usePlayer();

  switch (currentRoute) {
    case '/game':
      return player ? <GamePage /> : <LoginPage />;
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
