<script lang="ts" >
	import { joinWaitingList, leaveWaitingList } from "$lib/gameActions";
	import { afterGameModal, displayBoard, playerReady } from "$lib/stores";
	import { clearBoard } from "$lib/utils";
	import { Button, Spinner } from "flowbite-svelte";

    function handleFindGameButtonClick() {
        $playerReady = !$playerReady;
        if ($playerReady)
            joinWaitingList();
        else
            leaveWaitingList();
        
        if ($displayBoard) {
            clearBoard();
        }
    }

</script>

{#if !$displayBoard || (!$playerReady && !$afterGameModal)}

    <Button class="my-10" on:click={handleFindGameButtonClick}>
        {#if $playerReady}
            Cancel
        {:else}
            Find game
        {/if}
    </Button>
    
    {#if $playerReady}
        <p>
            <Spinner class="me-3" size="4"/>Searching for an opponent</p>
    {/if}
{/if}