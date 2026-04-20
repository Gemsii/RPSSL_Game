import { PlayGameRequest, PlayGameResponse } from '../models/Game';
import { ScoreboardEntry } from '../models/Scoreboard';

const BASE_URL = process.env.REACT_APP_API_URL;

export async function playGame(request: PlayGameRequest): Promise<PlayGameResponse> {
  const response = await fetch(`${BASE_URL}/games/play`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    const errors: string[] = await response.json().catch(() => ['Error playing game.']);
    throw new Error(errors[0] ?? 'Gameplay error.');
  }

  return response.json();
}

export async function getScoreboard(playerId: string, page: number = 1): Promise<ScoreboardEntry[]> {
  const response = await fetch(`${BASE_URL}/games/scoreboard/${playerId}?page=${page}`);
  if (!response.ok) throw new Error('Error loading scoreboard.');
  return response.json();
}

export async function resetScoreboard(playerId: string): Promise<void> {
  const response = await fetch(`${BASE_URL}/games/scoreboard/${playerId}`, {
    method: 'DELETE',
  });
  if (!response.ok) throw new Error('Error resetting scoreboard.');
}
