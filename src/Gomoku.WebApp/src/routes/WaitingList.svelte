<script>
    import { waitingList } from "$lib/stores";

    /**
	 * @type {any[]}
	 */
    let waitingList_value = [];

    waitingList.subscribe((val) => {
        waitingList_value = val;
    })
</script>

<div>
    <p>Waiting list:</p>
    <ul>
        {#each waitingList_value as playerWaiting}
            <li>{playerWaiting}</li>
        {/each}
    </ul>
</div>