<script>
    import { HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
    import { onMount, onDestroy, createEventDispatcher } from 'svelte';
    import WaitingList from "./WaitingList.svelte";
    import joinWaitingList from './WaitingList.svelte'
    import { waitingList, player } from "$lib/stores";
    import { v4 as uuid } from "uuid";
    

    const dispatch = createEventDispatcher();

    let playerReady = false;
    let wlComp;
    let playerName = uuid();
    player.set(playerName);

    const triggerPlayerReady = async () => {
        playerReady = !playerReady;
        console.log("playerReady:" + playerReady);
        console.log("playerName:" + playerName);

        if (playerReady){
            await connection.start();
            wlComp.joinWaitingList(playerName);
        } else {
            await connection.stop();
        }

    }
    const connection = new HubConnectionBuilder()
        .withUrl("http://localhost:5190/gameHub")
        .configureLogging(LogLevel.Information)
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

    // onMount(async () => {
    //     await connection.start();
    // });

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

<WaitingList bind:this={wlComp}/>