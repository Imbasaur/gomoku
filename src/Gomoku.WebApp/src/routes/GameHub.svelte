<script lang="ts" >
    import { HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
    import { onDestroy } from 'svelte';
    import { player, gameCode, moves, latestMove, displayBoard } from "$lib/stores";
	import type { Join } from "$lib/requests/join";

    let playerReady = false;

    const triggerPlayerReady = async () => {
        if (!playerReady && connection.state === HubConnectionState.Disconnected){
            await connection.start()
        } else if (connection.state === HubConnectionState.Connected) {
            await connection.stop();
        }
        playerReady = !playerReady;
    };

    const connection = new HubConnectionBuilder()
        .withUrl("http://localhost:5190/gameHub?username=" + $player)
        .configureLogging(LogLevel.Debug)
        .withAutomaticReconnect()
        .build();

    connection.on('PlayerJoinedWaitingList', name => {
        console.log('Player ' + name + ' joined waiting list.');
    })

    connection.on('PlayerLeftWaitingList', name => {
        console.log('Player ' + name + ' left waiting list.');
    })

    connection.on('GameCreated', name => {
        const request: Join = {code: name, playerName: $player}
        connection.invoke('Join', request)
        $gameCode = name
        console.log('Game ' + name + ' created.');
    })

    connection.on('PlayerConnected', name => {
        if (name == $player)
            $displayBoard = true;
        console.log('Player ' + name + ' connected.');
    })

    connection.on('PlayersConnected', () => {
        // todo: start the game etc
        console.log('Both players connected.');
    })

    connection.on('MoveAdded', move => {
        // todo: add move to board
        $latestMove = move;
        $moves = [...$moves, move];
        console.log('SignalR - Move added at ' + move + ' by ' + getcolor() + '.');
        console.log('SignalR - Moves so far: ' + $moves);
    })

    connection.on('GameFinished', winner => {
        console.log('SignalR - Game finished and the winner is  ' + winner);
        alert(winner + ' won the game!')
        // todo: disable board, allow to make new game
    })

    function getcolor(){
        if ($moves.length % 2 == 1)
            return "black"
        return "white"
    }

    onDestroy(async () => {
        await connection.stop();
    });
</script>

<button on:click={triggerPlayerReady}>
    {#if playerReady}
        Cancel
    {:else}
        Start
    {/if}
</button>