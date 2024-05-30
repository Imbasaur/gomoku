import { writable, readable } from "svelte/store";
import { uniqueNamesGenerator, adjectives, colors, animals } from 'unique-names-generator';
import type { Clock } from "$lib/types/Clock";

const name: string = uniqueNamesGenerator({
    dictionaries: [adjectives, colors, animals]
  }); 
  

export const player = readable<string>(name);

export const gameCode = writable<string>('');
export const moves = writable<string[]>([]);
export const latestMove = writable<string>('');
export const activePlayer = writable<string>('');
export const clock = writable<Clock>();

export const displayBoard = writable<boolean>(false);
export const playerReady = writable<boolean>(false);
export const gameFinished = writable<boolean>(false);