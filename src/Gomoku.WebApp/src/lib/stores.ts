import { writable, readable } from "svelte/store";
import { uniqueNamesGenerator, adjectives, animals } from 'unique-names-generator';
import type { Clock } from "$lib/types/Clock";
import type { Game } from "./types/Game";

const name: string = uniqueNamesGenerator({
  dictionaries: [adjectives, animals]
}); 
  
export const player = readable<string>(name);

export const gameInfo = writable<Game>();
export const moves = writable<string[]>([]);
export const latestMove = writable<string>('');
export const activePlayer = writable<string>('');
export const clock = writable<Clock>();

export const displayBoard = writable<boolean>(false);
export const playerReady = writable<boolean>(false);
export const gameFinished = writable<boolean>(false);
export const winningStones = writable<string[]>([]);
export const gameWinner = writable<string>('');

export const afterGameModal = writable<boolean>(false);