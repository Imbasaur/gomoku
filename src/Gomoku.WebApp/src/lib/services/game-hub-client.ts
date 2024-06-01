
import { HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
import { player, gameCode, moves, latestMove, displayBoard, clock, playerReady, activePlayer, gameFinished } from "$lib/stores";
import type { Join } from "$lib/requests/join";
import type { MoveAdded } from "$lib/responses/moveAdded";

// store values
let playerSub: string = "", movesSub: string[], gameFinishedSub: boolean

player.subscribe((value) => (playerSub = value));
moves.subscribe((value) => (movesSub = value));
gameFinished.subscribe((value) => (gameFinishedSub = value));

const connection = new HubConnectionBuilder()
    .withUrl("http://localhost:5190/gameHub?username=" + playerSub) // todo check if name is being sent
    .configureLogging(LogLevel.Debug)
    .withAutomaticReconnect()
    .build();

playerReady.subscribe((value) => {
    if (value && connection.state === HubConnectionState.Disconnected)
        connection.start()
    else if (connection.state === HubConnectionState.Connected)
        connection.stop();
})

connection.on('PlayerJoinedWaitingList', name => {
    console.log('Player ' + name + ' joined waiting list.');
})

connection.on('PlayerLeftWaitingList', name => {
    console.log('Player ' + name + ' left waiting list.');
})

connection.on('GameCreated', name => {
    const request: Join = {code: name, playerName: playerSub}
    connection.invoke('Join', request)
    gameCode.set(name)
    console.log('Game ' + name + ' created.');
})

connection.on('PlayerConnected', name => {
    if (name == playerSub)
        displayBoard.set(true)
    console.log('Player ' + name + ' connected.');
})

connection.on('PlayersConnected', () => {
    // todo: start the game etc
    console.log('Both players connected.');
})

connection.on('MoveAdded', (response: MoveAdded) => {
    latestMove.set(response.move)
    moves.update(items => ([...items, response.move]))
    clock.set(response.clock)
    activePlayer.set(getActivePlayer());
    console.log('SignalR - Clock:' + JSON.stringify(response.clock));
    console.log('SignalR - Move added at ' + response.move + ' by ' + getcolor() + '.');
    console.log('SignalR - Moves so far: ' + movesSub);
})

connection.on('GameFinished', winner => {
    if (!gameFinishedSub){
        console.log('SignalR - Game finished and the winner is  ' + winner);
        alert(winner + ' won the game!')
        gameFinished.set(true);
    }
    // todo: disable board, allow to make new game
})

connection.on('GameFinishedByPlayerTimeout', winner => {
    if (!gameFinishedSub){
        console.log('SignalR - Game finished by player timeout and the winner is  ' + winner);
        alert(winner + ' won the game!')
        gameFinished.set(true);
    }
    // todo: disable board, allow to make new game
})

function getcolor(){
    if (movesSub.length % 2 == 1)
        return "black"
    return "white"
}

function getActivePlayer(){
    if (movesSub.length % 2 == 0)
        return "black"
    return "white"
}

// todo fix error Function called outside component initialization
// onDestroy(async () => {  
//     await connection.stop();
// });

export async function move(code:string, move:string){
    try {
        const request = { code: code, move: move };
        await connection.invoke("Move", request);
    } catch (error) {
        console.error("An unexpected error occurred:", error);
    }
}