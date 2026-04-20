import { createContext, useContext, useState, useCallback, ReactNode } from 'react';

export interface Player {
  id?: string;
  name: string;
  isAnonymous: boolean;
}

interface PlayerContextValue {
  player: Player | null;
  setPlayer: (player: Player) => void;
  clearPlayer: () => void;
}

const PlayerContext = createContext<PlayerContextValue | null>(null);

export function PlayerProvider({ children }: { children: ReactNode }) {
  const [player, setPlayerState] = useState<Player | null>(null);

  const setPlayer = useCallback((p: Player) => {
    setPlayerState(p);
  }, []);

  const clearPlayer = useCallback(() => {
    setPlayerState(null);
  }, []);

  return (
    <PlayerContext.Provider value={{ player, setPlayer, clearPlayer }}>
      {children}
    </PlayerContext.Provider>
  );
}

export function usePlayer(): PlayerContextValue {
  const context = useContext(PlayerContext);
  if (!context) {
    throw new Error('usePlayer must be used within PlayerProvider');
  }
  return context;
}
