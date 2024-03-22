<script>
    import { HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
    import { onDestroy, createEventDispatcher } from 'svelte';
    import WaitingList from "./WaitingList.svelte";
    import joinWaitingList from './WaitingList.svelte'
    import { waitingList, player } from "$lib/stores";
    

    const dispatch = createEventDispatcher();

    let playerReady = false;
    let wlComp;

    const triggerPlayerReady = async () => {
        playerReady = !playerReady;

        if (playerReady){
            await connection.start()
            .then(() => {
                wlComp.joinWaitingList(connection.connectionId); // use connectionId as player name, we will do proper user later
                player.set(connection.connectionId);
            })
        } else {
            await connection.stop();
        }
    }

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
        const index = $waitingList.indexOf(name, 0);
        if (index > -1) {
            $waitingList = [...$waitingList.slice(0, index), ...$waitingList.slice(index + 1)]; // filter? 
        }
        console.log('Player ' + name + ' left waiting list.');
    })

    onDestroy(async () => {
        await connection.stop();
    });
    
    export function createGame() {
        fetch("http://localhost:5190/Game", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to create game.')
            }
            let data = response.json();
            console.log("Successfully created game.");
            $waitingList = $waitingList.filter(player =>
            player !== data.blackName && player !== data.whiteName
            );
        })
        .catch(error => {
            console.error('Error creating game:', error.message);
        });
    }

    waitingList.subscribe((val) => {
        if (val.length > 1){
            createGame()
        }
    })
</script>

<div>
    <p>Player name is {$player}</p>
</div>

<button on:click={triggerPlayerReady}>
    {#if playerReady}
        Cancel
    {:else}
        Start
    {/if}
</button>

<WaitingList bind:this={wlComp}/>