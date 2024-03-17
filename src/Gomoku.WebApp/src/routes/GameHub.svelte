<script>
    import {HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
    import { onMount, onDestroy, createEventDispatcher } from 'svelte';

    /**
	 * @type {string | any[]}
	 */
    let waitingList = [];
    const dispatch = createEventDispatcher();  

    const connection = new HubConnectionBuilder()
        .withUrl("http://localhost:5190/gameHub")
        .configureLogging(LogLevel.Information)
        .withAutomaticReconnect()
        .build();

    connection.on('PlayerJoinedWaitingList', name => {
        waitingList =[...waitingList, name];
        console.log('Player ' + name + ' joined waiting list.');
    })

    connection.on('PlayerLeftWaitingList', name => {
        const index = waitingList.indexOf(name, 0);
        if (index > -1) {
            waitingList = [...waitingList.slice(0, index), ...waitingList.slice(index + 1)]; // filter? 
        }
        console.log('Player ' + name + ' left waiting list.');
    })

    onMount(async () => {
        await connection.start();
    });

    onDestroy(async () => {
        await connection.stop();
    });
    
</script>

<div>
    <p>Waiting list:</p>
    <ul>
        {#each waitingList as playerWaiting}
            <li>{playerWaiting}</li>
        {/each}
    </ul>
</div>