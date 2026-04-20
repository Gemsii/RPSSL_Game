import { useState } from 'react';
import type { SubmitEvent } from 'react';
import { useRouter } from '../router/RouterContext';
import { usePlayer } from '../context/PlayerContext';
import { createPlayer } from '../api/playersApi';
import './LoginPage.css';

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
      <div className="login-page">
        <div className="login-card">
          <span className="login-card__icon">🎮</span>
          <h1 className="login-card__title">RPSSL Game</h1>
          <p className="login-card__subtitle">Enter your name to track your score</p>
          <form className="register-form" onSubmit={handleRegister}>
            <input
              className="input"
              type="text"
              placeholder="Enter your name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              disabled={loading}
              required
            />
            <button type="submit" className="btn btn--primary btn--full" disabled={loading}>
              {loading ? 'Creating...' : 'Create Player and Play'}
            </button>
            <button
              type="button"
              className="btn btn--ghost btn--full"
              onClick={() => { setMode('select'); setError(''); }}
            >
              Back
            </button>
          </form>
          {error && <p className="error-text">{error}</p>}
        </div>
      </div>
    );
  }

  return (
    <div className="login-page">
      <div className="login-card">
        <span className="login-card__icon">🎮</span>
        <h1 className="login-card__title">RPSSL Game</h1>
        <p className="login-card__subtitle">Rock · Paper · Scissors · Spock · Lizard</p>
        <div className="login-card__actions">
          <button className="btn btn--primary btn--full" onClick={handleAnonymous}>
            Play as Anonymous
          </button>
          <button className="btn btn--secondary btn--full" onClick={() => setMode('register')}>
            Create Player
          </button>
        </div>
      </div>
    </div>
  );
}
