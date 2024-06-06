
import { HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
import { player, moves, latestMove, displayBoard, clock, playerReady, activePlayer, gameFinished, winningStones, gameInfo, gameWinner } from "$lib/stores";
import type { Join } from "$lib/requests/join";
import type { MoveAdded } from "$lib/responses/moveAdded";
import type { GameFinished } from "$lib/responses/gameFinished";
import type { Game } from "$lib/types/Game";
import type { Clock } from "$lib/types/Clock";
import { PUBLIC_BACKEND_ADDRESS } from '$env/static/public';

// store values
let playerSub: string = "", movesSub: string[], gameFinishedSub: boolean

player.subscribe((value) => (playerSub = value));
moves.subscribe((value) => (movesSub = value));
gameFinished.subscribe((value) => (gameFinishedSub = value));

const connection = new HubConnectionBuilder()
    .withUrl(PUBLIC_BACKEND_ADDRESS + "/gameHub?username=" + playerSub) // todo check if name is being sent
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
    // todo remove?
})

connection.on('PlayerLeftWaitingList', name => {
    // todo remove?
})

connection.on('GameCreated', (game: Game) => {
    const request: Join = {code: game.code, playerName: playerSub}
    connection.invoke('Join', request)
    gameInfo.set(game)
    clock.set({ black: game.time, white: game.time})
})

connection.on('PlayerConnected', name => {
    if (name == playerSub)
        displayBoard.set(true)
})

connection.on('PlayersConnected', () => {
    // todo: start the game etc
})

connection.on('MoveAdded', (response: MoveAdded) => {
    latestMove.set(response.move)
    moves.update(items => ([...items, response.move]))
    clock.set(response.clock)
    activePlayer.set(getActivePlayer());
})

connection.on('GameFinished', (response: GameFinished) => {
    if (!gameFinishedSub){
        winningStones.set(response.winningStones)
        gameWinner.set(response.winner)
        gameFinished.set(true)
        playerReady.set(false) // todo: should probably keep connection little longer for rematch scenario
    }
    // todo: disable board, allow to make new game
})

connection.on('GameFinishedByPlayerTimeout', winner => {
    if (!gameFinishedSub){
        gameWinner.set(winner)
        gameFinished.set(true);
        playerReady.set(false) // todo: should probably keep connection little longer for rematch scenario
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