import { useState, useEffect } from 'react';
import { ScoreboardEntry } from '../models/Scoreboard';
import { getScoreboard, resetScoreboard } from '../api/gamesApi';

interface ScoreboardProps {
  readonly playerId: string;
  readonly choiceNames: Record<number, string>;
  readonly refreshKey: number;
}

export default function Scoreboard({ playerId, choiceNames, refreshKey }: ScoreboardProps) {
  const [entries, setEntries] = useState<ScoreboardEntry[]>([]);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const [resetting, setResetting] = useState(false);
  const [error, setError] = useState('');
  const [localRefreshKey, setLocalRefreshKey] = useState(0);

  useEffect(() => {
    setLoading(true);
    setError('');

    getScoreboard(playerId, page)
      .then(setEntries)
      .catch(() => setError('Error loading scoreboard.'))
      .finally(() => setLoading(false));
  }, [playerId, page, refreshKey, localRefreshKey]);

  async function handleReset() {
    if (!globalThis.confirm('Are you sure you want to delete all results?')) return;
    setResetting(true);
    setError('');
    try {
      await resetScoreboard(playerId);
      setEntries([]);
      setPage(1);
      setLocalRefreshKey((k) => k + 1);
    } catch {
      setError('Error resetting scoreboard.');
    } finally {
      setResetting(false);
    }
  }

  return (
    <div>
      <h2>Scoreboard (last 10)</h2>

      {loading && <p>Loading...</p>}
      {error && <p>{error}</p>}

      {!loading && !error && entries.length === 0 && (
        <p>No games played yet.</p>
      )}

      {!loading && entries.length > 0 && (
        <table>
          <thead>
            <tr>
              <th>#</th>
              <th>Your move</th>
              <th>Computer</th>
              <th>Result</th>
            </tr>
          </thead>
          <tbody>
            {entries.map((entry, index) => (
              <tr key={index}>
                <td>{(page - 1) * 10 + index + 1}</td>
                <td>{choiceNames[entry.playerChoice] ?? entry.playerChoice}</td>
                <td>{choiceNames[entry.computerChoice] ?? entry.computerChoice}</td>
                <td>{entry.results}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      <div>
        <button onClick={() => setPage((p) => Math.max(1, p - 1))} disabled={page === 1 || loading}>
          Previous
        </button>
        <span> Page {page} </span>
        <button onClick={() => setPage((p) => p + 1)} disabled={entries.length < 10 || loading}>
          Next
        </button>
      </div>

      <button onClick={handleReset} disabled={resetting || loading}>
        {resetting ? 'Resetting...' : 'Reset Scoreboard'}
      </button>
    </div>
  );
}
