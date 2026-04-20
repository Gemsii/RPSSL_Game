import { useState } from 'react';
import { useRouter } from '../router/RouterContext';
import { usePlayer } from '../context/PlayerContext';
import { getRandomChoice } from '../api/choicesApi';
import { playGame } from '../api/gamesApi';
import { PlayGameResponse } from '../models/Game';
import { useChoices } from '../hooks/useChoices';
import Scoreboard from '../components/Scoreboard';
import './GamePage.css';

const CHOICE_EMOJI: Record<string, string> = {
  rock: '✊',
  paper: '🖐️',
  scissors: '✌️',
  spock: '🖖',
  lizard: '🦎',
};

function getEmoji(name: string): string {
  return CHOICE_EMOJI[name.toLowerCase()] ?? '🎲';
}

function getResultClass(result: string): string {
  const r = result.toLowerCase();
  if (r === 'win') return 'result-card__badge--win';
  if (r === 'lose') return 'result-card__badge--lose';
  return 'result-card__badge--tie';
}

export default function GamePage() {
  const { navigate } = useRouter();
  const { player } = usePlayer();
  const { choices, choiceNames, loading: loadingChoices, error: choicesError } = useChoices();

  const [selectedChoice, setSelectedChoice] = useState<number | null>(null);
  const [result, setResult] = useState<PlayGameResponse | null>(null);
  const [loadingRandom, setLoadingRandom] = useState(false);
  const [loadingPlay, setLoadingPlay] = useState(false);
  const [error, setError] = useState('');
  const [scoreboardRefreshKey, setScoreboardRefreshKey] = useState(0);

  async function handleRandomChoice() {
    setError('');
    setLoadingRandom(true);
    try {
      const random = await getRandomChoice();
      setSelectedChoice(random.id);
    } catch {
      setError('Error getting random move.');
    } finally {
      setLoadingRandom(false);
    }
  }

  async function handlePlay() {
    if (selectedChoice === null) return;
    setError('');
    setResult(null);
    setLoadingPlay(true);
    try {
      const response = await playGame({
        choice: selectedChoice,
        playerId: player?.isAnonymous ? undefined : player?.id,
      });
      setResult(response);
      if (!player?.isAnonymous) {
        setScoreboardRefreshKey((k) => k + 1);
      }
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : 'Error playing game.');
    } finally {
      setLoadingPlay(false);
    }
  }

  function handlePlayAgain() {
    setResult(null);
    setSelectedChoice(null);
  }

  return (
    <div className="game-page">
      <div className="game-header">
        <h1 className="game-header__title">RPSSL Game</h1>
        <div className="game-header__player">
          <span className="player-chip">
            <span className="player-chip__icon">👤</span>
            {player?.name ?? 'Anonymous'}
          </span>
          <button className="back-link" onClick={() => navigate('/')}>
            ← Back
          </button>
        </div>
      </div>

      {(choicesError || error) && <p className="error-text">{choicesError || error}</p>}
      {loadingChoices && <p className="status-text">Loading moves...</p>}

      {!loadingChoices && !result && (
        <div className="card">
          <p className="card__title">Choose your move</p>
          <div className="choices-grid">
            {choices.map((choice) => (
              <button
                key={choice.id}
                className={`choice-card${selectedChoice === choice.id ? ' choice-card--selected' : ''}`}
                onClick={() => setSelectedChoice(choice.id)}
                disabled={loadingPlay}
              >
                <span className="choice-card__emoji">{getEmoji(choice.name)}</span>
                <span className="choice-card__name">{choice.name}</span>
              </button>
            ))}
          </div>

          <div className="game-actions">
            <button
              className="btn btn--secondary"
              onClick={handleRandomChoice}
              disabled={loadingRandom || loadingPlay}
            >
              🎲 {loadingRandom ? 'Choosing...' : 'Random'}
            </button>
            <button
              className="btn btn--primary"
              onClick={handlePlay}
              disabled={selectedChoice === null || loadingPlay}
            >
              {loadingPlay ? 'Playing...' : 'Play ▶'}
            </button>
          </div>
        </div>
      )}

      {result && (
        <div className="result-card">
          <div className={`result-card__badge ${getResultClass(result.results)}`}>
            {result.results}
          </div>
          <div className="result-card__moves">
            <div className="result-card__move">
              <span className="result-card__move-label">You</span>
              <span className="result-card__move-emoji">{getEmoji(choiceNames[result.player] ?? '')}</span>
              <span className="result-card__move-name">{choiceNames[result.player]}</span>
            </div>
            <span className="result-vs">vs</span>
            <div className="result-card__move">
              <span className="result-card__move-label">Computer</span>
              <span className="result-card__move-emoji">{getEmoji(choiceNames[result.computer] ?? '')}</span>
              <span className="result-card__move-name">{choiceNames[result.computer]}</span>
            </div>
          </div>
          <button className="btn btn--primary" onClick={handlePlayAgain}>
            Play Again
          </button>
        </div>
      )}

      {!player?.isAnonymous && player?.id && (
        <Scoreboard
          playerId={player.id}
          choiceNames={choiceNames}
          refreshKey={scoreboardRefreshKey}
        />
      )}
    </div>
  );
}
