<script lang="ts">
    import { HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
    import { onDestroy, createEventDispatcher } from 'svelte';
    import WaitingList from "./WaitingList.svelte";
    import { waitingList, player } from "$lib/stores";
    

    const dispatch = createEventDispatcher();

    let playerReady = false;
    let wlComp;

    const triggerPlayerReady = async () => {
        if (!playerReady && connection.state === HubConnectionState.Disconnected){
            await connection.start()
            await wlComp.joinWaitingList(connection.connectionId)
            player.set(connection.connectionId); // use connectionId as player name, we will do proper user later
        } else if (connection.state === HubConnectionState.Connected) {
            if ($waitingList.find(x => x === connection.connectionId)){
                await wlComp.removeFromWaitingList(connection.connectionId);
                removeFromWaitingListLocal(connection.connectionId);
            }
            await connection.stop();
        }
        playerReady = !playerReady;
    };

    const connection = new HubConnectionBuilder()
        .withUrl("http://localhost:5190/gameHub")
        .configureLogging(LogLevel.Debug)
        .withAutomaticReconnect()
        .build();

    connection.on('PlayerJoinedWaitingList', name => {
        $waitingList = [...$waitingList, name];
        console.log('Player ' + name + ' joined waiting list.');
    })

    connection.on('PlayerLeftWaitingList', name => {
        removeFromWaitingListLocal(name);
        console.log('Player ' + name + ' left waiting list.');
    })

    connection.on('GameCreated', name => {
        joinGame(name, $player)
        console.log('Game ' + name + ' created.');
    })

    connection.on('PlayerConnected', name => {
        // nothing happens here for now
        console.log('Player ' + name + ' connected.');
    })

    connection.on('PlayersConnected', () => {
        // todo: display board, start the game etc
        console.log('Both players connected.');
    })

    function removeFromWaitingListLocal(name: string){
        const index = $waitingList.indexOf(name, 0);
        if (index > -1) {
            $waitingList = [...$waitingList.slice(0, index), ...$waitingList.slice(index + 1)]; // filter? 
        }
    }

    onDestroy(async () => {
        await connection.stop();
    });
    
    export function createGame() { // do i even need to call this from front? no needed in quckfinding
        fetch("http://localhost:5190/Game", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to create the game.')
            }
            let data = response.json();
            console.log("Successfully created the game.");
            $waitingList = $waitingList.filter(player => player !== data.blackName && player !== data.whiteName);
            console.log($waitingList)
        })
        .catch(error => {
            console.error('Error creating the game:', error.message);
        });
    }
    
    export function joinGame(code: string, playerName: string) { 
        fetch("http://localhost:5190/Game/join", {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ code, playerName})
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to join the game')
            }
            console.log("Successfully joined the game.");
            // anything else?
        })
        .catch(error => {
            console.error('Error creating game:', error.message);
        });
    }
    
    export function addMove(code: string, x: number, y: number) { 
        fetch("http://localhost:5190/Game/move", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ code, x, y})
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to add a move to the game')
            }
            console.log("Successfully added move the game.");
            // anything else?
        })
        .catch(error => {
            console.error('Error adding move to the game:', error.message);
        });
    }
</script>

{#if $player != ''}
<div>
    <p>Player name is {$player}</p>
</div>
{/if}

<button on:click={triggerPlayerReady}>
    {#if playerReady}
        Cancel
    {:else}
        Start
    {/if}
</button>

<WaitingList bind:this={wlComp}/>