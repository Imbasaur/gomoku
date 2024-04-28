import { writable, readable } from "svelte/store";

export const waitingList = writable([]);

export const player = writable('');
export const gameCode = writable('');