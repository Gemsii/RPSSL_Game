import { CreatePlayerRequest, CreatePlayerResponse } from '../models/Player';

const BASE_URL = process.env.REACT_APP_API_URL;

export async function createPlayer(request: CreatePlayerRequest): Promise<CreatePlayerResponse> {
  const response = await fetch(`${BASE_URL}/players`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    const body = await response.json().catch(() => null);
    // 409 / domain errors → { error: "..." }
    // 400 validation errors → ["...", "..."]
    const message =
      (body && typeof body.error === 'string' && body.error) ||
      (Array.isArray(body) && typeof body[0] === 'string' && body[0]) ||
      'Error creating player.';
    throw new Error(message);
  }

  return response.json();
}
