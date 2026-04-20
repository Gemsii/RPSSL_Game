import { useState, useEffect } from 'react';
import { ScoreboardEntry } from '../models/Scoreboard';
import { getScoreboard, resetScoreboard } from '../api/gamesApi';
import './Scoreboard.css';

interface ScoreboardProps {
  readonly playerId: string;
  readonly choiceNames: Record<number, string>;
  readonly refreshKey: number;
}

function getResultBadgeClass(result: string): string {
  const r = result.toLowerCase();
  if (r === 'win') return 'result-badge result-badge--win';
  if (r === 'lose') return 'result-badge result-badge--lose';
  return 'result-badge result-badge--tie';
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
    <div className="scoreboard">
      <div className="scoreboard__header">
        <h2 className="scoreboard__title">Scoreboard</h2>
        <button className="btn btn--danger" onClick={handleReset} disabled={resetting || loading}>
          {resetting ? 'Resetting...' : 'Reset'}
        </button>
      </div>

      {loading && <p className="scoreboard__status">Loading...</p>}
      {error && <p className="scoreboard__error">{error}</p>}

      {!loading && !error && entries.length === 0 && (
        <p className="scoreboard__empty">No games played yet.</p>
      )}

      {!loading && entries.length > 0 && (
        <div className="scoreboard__table-wrapper">
          <table className="scoreboard__table">
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
                <tr key={`${page}-${index}`}>
                  <td>{(page - 1) * 10 + index + 1}</td>
                  <td>{choiceNames[entry.playerChoice] ?? entry.playerChoice}</td>
                  <td>{choiceNames[entry.computerChoice] ?? entry.computerChoice}</td>
                  <td>
                    <span className={getResultBadgeClass(entry.results)}>
                      {entry.results}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      <div className="scoreboard__pagination">
        <button
          className="pagination-btn"
          onClick={() => setPage((p) => Math.max(1, p - 1))}
          disabled={page === 1 || loading}
        >
          ← Previous
        </button>
        <span className="pagination-info">Page {page}</span>
        <button
          className="pagination-btn"
          onClick={() => setPage((p) => p + 1)}
          disabled={entries.length < 10 || loading}
        >
          Next →
        </button>
      </div>
    </div>
  );
}
