import { get } from 'svelte/store';
import { connection } from '$lib/gameHub';
import { HubConnectionState } from '@microsoft/signalr';

// Function to send a move to the server
export async function sendMove(code: string, move: string) {
    try {
        const hubConnection = get(connection);

        if (!hubConnection || hubConnection.state !== HubConnectionState.Connected) {
            console.error("No connection available to send move");
            return;
        }

        const request = { code, move };
        await hubConnection.invoke("Move", request); 
    } catch (error) {
        console.error("Error while sending move:", error);
    }
}

// Example of another server method
export async function joinWaitingList() {
    try {
        const hubConnection = get(connection);

        if (!hubConnection || hubConnection.state !== HubConnectionState.Connected) {
            console.error("No connection available to join waiting list");
            return;
        }

        await hubConnection.invoke("JoinWaitingList");
    } catch (error) {
        console.error("Error while joining game:", error);
    }
}

export async function joinGame(code: string, playerName: string) {
    try {
        const hubConnection = get(connection);

        if (!hubConnection || hubConnection.state !== HubConnectionState.Connected) {
            console.error("No connection available to join game");
            return;
        }

        const request = { code, playerName };
        await hubConnection.invoke("JoinGame", request);
    } catch (error) {
        console.error("Error while joining game:", error);
    }
}