<script lang="ts">
    import { waitingList } from "$lib/stores";


    let waitingList_value: any[] = [];

    waitingList.subscribe((val) => {
        waitingList_value = val;
        if (val.length > 1){
            console.log("Two players waiting for game - creating one.");
        }
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
            console.log("Successfully joined waiting list");
        })
        .catch(error => {
            console.error('Error joining waiting list:', error.message);
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