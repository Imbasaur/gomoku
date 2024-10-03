import { writable, readable } from "svelte/store";
import { uniqueNamesGenerator, adjectives, animals } from 'unique-names-generator';
import type { Clock } from "$lib/types/Clock";
import type { Game } from "./types/Game";
import { PUBLIC_BACKEND_ADDRESS } from "$env/static/public";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

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

// Create a writable store to hold the connection
export const connection = writable<signalR.HubConnection | null>(null);

export function createHubConnection() {
    // Initialize the SignalR connection
    const hubConnection = new HubConnectionBuilder()
        .withUrl(PUBLIC_BACKEND_ADDRESS + "/gameHub?username=" + name)
        .configureLogging(LogLevel.Debug)
        .build();

    // Start the connection and update the store
    hubConnection.start()
        .then(() => {
            console.log("SignalR Connection started");
            connection.set(hubConnection);
        })
        .catch(err => console.error("Error starting SignalR connection:", err));
}