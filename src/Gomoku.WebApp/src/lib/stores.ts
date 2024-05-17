import { writable, readable } from "svelte/store";
import { uniqueNamesGenerator, adjectives, colors, animals } from 'unique-names-generator';

const name: string = uniqueNamesGenerator({
    dictionaries: [adjectives, colors, animals]
  }); 
  

export const player = readable(name);

export const gameCode = writable('');
export const moves = writable([]);
export const latestMove = writable('');
export const displayBoard = writable(false);