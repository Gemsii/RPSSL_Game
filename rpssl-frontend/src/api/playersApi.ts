import { CreatePlayerRequest, CreatePlayerResponse } from '../models/Player';

const BASE_URL = process.env.REACT_APP_API_URL;

export async function createPlayer(request: CreatePlayerRequest): Promise<CreatePlayerResponse> {
  const response = await fetch(`${BASE_URL}/players`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    const errors: string[] = await response.json().catch(() => ['Error creating player.']);
    throw new Error(errors[0] ?? 'Error creating player.');
  }

  return response.json();
}
