import { useState, useEffect } from 'react';
import { Choice } from '../models/Choice';
import { getChoices } from '../api/choicesApi';

interface UseChoicesResult {
  choices: Choice[];
  choiceNames: Record<number, string>;
  loading: boolean;
  error: string;
}

export function useChoices(): UseChoicesResult {
  const [choices, setChoices] = useState<Choice[]>([]);
  const [choiceNames, setChoiceNames] = useState<Record<number, string>>({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    getChoices()
      .then((data) => {
        setChoices(data);
        const map: Record<number, string> = {};
        data.forEach((c) => { map[c.id] = c.name; });
        setChoiceNames(map);
      })
      .catch(() => setError('Error loading moves.'))
      .finally(() => setLoading(false));
  }, []);

  return { choices, choiceNames, loading, error };
}
