import { writable, get } from 'svelte/store';
import * as signalR from '@microsoft/signalr';
import { activePlayer, clock, displayBoard, gameFinished, gameInfo, gameWinner, latestMove, moves, player, playerReady, winningStones } from '$lib/stores';
import { PUBLIC_BACKEND_ADDRESS } from '$env/static/public';
import { joinGame } from './gameActions';
import { HubConnectionState } from '@microsoft/signalr';

export const connection = writable<signalR.HubConnection | null>(null);

// Connection
export function createHubConnection() {
    const playerName = get(player);

    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(`${PUBLIC_BACKEND_ADDRESS}/gameHub?username=${playerName}`)
        .configureLogging(signalR.LogLevel.Debug)
        .build();

        hubConnection.start()
        .then(() => {
            console.log("SignalR Connection started successfully");
            connection.set(hubConnection);
            setupEventHandlers(hubConnection);
        })
        .catch(err => {
            console.error("Error starting SignalR connection:", err);
        });

    return hubConnection;
}

export function stopHubConnection() {
    const hubConnection = get(connection);
    if (hubConnection && hubConnection.state === HubConnectionState.Connected) {
        hubConnection.stop()
            .then(() => {
                console.log("SignalR Connection stopped successfully");
                connection.set(null);
            })
            .catch(err => {
                console.error("Error stopping SignalR connection:", err);
            });
    }
}

// Event handlers
function setupEventHandlers(hubConnection: signalR.HubConnection) {
    hubConnection.on('PlayerJoinedWaitingList', name => {
        console.log(`${name} joined the waiting list`);
    });

    hubConnection.on('PlayerLeftWaitingList', name => {
        console.log(`${name} left the waiting list`);
    });

    hubConnection.on('GameCreated', game => {
        console.log('Game created:', game);
        gameInfo.set(game);
        clock.set({ black: game.time, white: game.time });
        joinGame(game.code, get(player))
    });

    hubConnection.on('PlayerConnected', name => {
        console.log('name: '+name+', player: '+player+', eq: '+(name == get(player)))
        if (name == get(player))
            displayBoard.set(true)
    })
    
    hubConnection.on('PlayersConnected', () => {
        // todo: start the game etc
    })

    hubConnection.on('MoveAdded', response => {
        latestMove.set(response.move);
        moves.update(items => [...items, response.move]);
        clock.set(response.clock);
        activePlayer.set(getActivePlayer());
    });

    hubConnection.on('GameFinished', response => {
        console.log('Game finished:', response);
        winningStones.set(response.winningStones);
        gameWinner.set(response.winner);
        gameFinished.set(true);
        playerReady.set(false);
    });

    hubConnection.on('GameFinishedByPlayerTimeout', response => {
        console.log('Game finished by player timeout:', response);
        if (!get(gameFinished)){
            gameWinner.set(response)
            gameFinished.set(true);
            playerReady.set(false) // todo: should probably keep connection little longer for rematch scenario
        }
        // todo: disable board, allow to make new game
    });
}

// Helper function
function getActivePlayer() {
    return get(moves).length % 2 === 0 ? "black" : "white";
}