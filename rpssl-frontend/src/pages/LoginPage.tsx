import { useState } from 'react';
import type { SubmitEvent } from 'react';
import { useRouter } from '../router/RouterContext';
import { usePlayer } from '../context/PlayerContext';
import { createPlayer } from '../api/playersApi';

type Mode = 'select' | 'register';

export default function LoginPage() {
  const { navigate } = useRouter();
  const { setPlayer } = usePlayer();

  const [mode, setMode] = useState<Mode>('select');
  const [name, setName] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  function handleAnonymous() {
    setPlayer({ name: 'Anonymous', isAnonymous: true });
    navigate('/game');
  }

  async function handleRegister(e: SubmitEvent<HTMLFormElement>) {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      const result = await createPlayer({ name });
      setPlayer({ id: result.id, name: result.name, isAnonymous: false });
      navigate('/game');
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : 'Error creating player.');
    } finally {
      setLoading(false);
    }
  }

  if (mode === 'register') {
    return (
      <div>
        <h1>RPSSL Game</h1>
        <form onSubmit={handleRegister}>
          <input
            type="text"
            placeholder="Enter your name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            disabled={loading}
            required
          />
          <button type="submit" disabled={loading}>
            {loading ? 'Creating...' : 'Create Player and Play'}
          </button>
          <button type="button" onClick={() => { setMode('select'); setError(''); }}>
            Back
          </button>
        </form>
        {error && <p>{error}</p>}
      </div>
    );
  }

  return (
    <div>
      <h1>RPSSL Game</h1>
      <button onClick={handleAnonymous}>Play as Anonymous</button>
      <button onClick={() => setMode('register')}>Create Player</button>
    </div>
  );
}
