<script lang="ts">
    import { player, waitingList } from "$lib/stores";
    import createGame from './GameHub.svelte';


    let waitingList_value: any[] = [];

    waitingList.subscribe((val) => {
        waitingList_value = val;
    })

    export function joinWaitingList(playerName: string) {
        fetch("http://localhost:5190/WaitingList/join", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ playerName })
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to join waiting list.')
            }
            console.log("Successfully joined waiting list.");
            $waitingList.push(playerName);
        })
        .catch(error => {
            console.error('Error joining waiting list:', error.message);
        });
    }

    export function removeFromWaitingList(playerName: string) {
        fetch("http://localhost:5190/WaitingList/" + playerName, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            },
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to remove from waiting list.')
            }
            console.log("Successfully removed from waiting list.");
            $waitingList.push(playerName);
        })
        .catch(error => {
            console.error('Error removing from waiting list:', error.message);
        });
    }
</script>

<div>
    <p>Waiting list:</p>
    <ul>
        {#each waitingList_value as playerWaiting}
            <li>{playerWaiting}</li>
        {/each}
    </ul>
</div>