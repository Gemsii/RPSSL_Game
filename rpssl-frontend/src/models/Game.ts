export interface PlayGameRequest {
  choice: number;
  playerId?: string;
}

export interface PlayGameResponse {
  results: string;
  player: number;
  computer: number;
}
