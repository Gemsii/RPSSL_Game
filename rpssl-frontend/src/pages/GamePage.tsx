import { useState } from 'react';
import { useRouter } from '../router/RouterContext';
import { usePlayer } from '../context/PlayerContext';
import { getRandomChoice } from '../api/choicesApi';
import { playGame } from '../api/gamesApi';
import { PlayGameResponse } from '../models/Game';
import { useChoices } from '../hooks/useChoices';
import Scoreboard from '../components/Scoreboard';

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
    <div>
      <h1>RPSSL Game</h1>
      <p>Playing as: <strong>{player?.name ?? 'Anonymous'}</strong></p>

      {loadingChoices && <p>Loading moves...</p>}

      {!loadingChoices && !result && (
        <>
          <div>
            {choices.map((choice) => (
              <button
                key={choice.id}
                onClick={() => setSelectedChoice(choice.id)}
                disabled={loadingPlay}
                style={{ fontWeight: selectedChoice === choice.id ? 'bold' : 'normal' }}
              >
                {choice.name}
              </button>
            ))}
          </div>

          <div>
            <button onClick={handleRandomChoice} disabled={loadingRandom || loadingPlay}>
              {loadingRandom ? 'Choosing...' : 'Choose Random'}
            </button>
            <button onClick={handlePlay} disabled={selectedChoice === null || loadingPlay}>
              {loadingPlay ? 'Playing...' : 'Play'}
            </button>
          </div>

          {selectedChoice !== null && (
            <p>Your choice: <strong>{choiceNames[selectedChoice]}</strong></p>
          )}
        </>
      )}

      {result && (
        <div>
          <h2>Result: {result.results}</h2>
          <p>Your move: <strong>{choiceNames[result.player]}</strong></p>
          <p>Computer's move: <strong>{choiceNames[result.computer]}</strong></p>
          <button onClick={handlePlayAgain}>Play Again</button>
        </div>
      )}

      {(choicesError || error) && <p>{choicesError || error}</p>}

      {!player?.isAnonymous && player?.id && (
        <Scoreboard
          playerId={player.id}
          choiceNames={choiceNames}
          refreshKey={scoreboardRefreshKey}
        />
      )}

      <button onClick={() => navigate('/')}>Back</button>
    </div>
  );
}
