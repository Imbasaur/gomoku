<script>
    import {HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
    import { onMount, onDestroy, createEventDispatcher } from 'svelte';

    export let player = '';
    const dispatch = createEventDispatcher();  

    const connection = new HubConnectionBuilder()
        .withUrl("http://localhost:5190/gameHub")
        .configureLogging(LogLevel.Information)
        .withAutomaticReconnect()
        .build();

    connection.on("PlayerJoinedWaitingList", player => {
        console.log("Player " + player + ' joined waiting list.');
        this.player = player;
    })

    onMount(async () => {
        await connection.start();
    });

    onDestroy(async () => {
        await connection.stop();
    });
    
</script>

<div>
    <p>{player}</p>
</div>