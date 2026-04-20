import { Choice } from '../models/Choice';

const BASE_URL = process.env.REACT_APP_API_URL;

export async function getChoices(): Promise<Choice[]> {
  const response = await fetch(`${BASE_URL}/choices`);
  if (!response.ok) throw new Error('Error loading moves.');
  return response.json();
}

export async function getRandomChoice(): Promise<Choice> {
  const response = await fetch(`${BASE_URL}/choices/random`);
  if (!response.ok) throw new Error('Error getting random move.');
  return response.json();
}
