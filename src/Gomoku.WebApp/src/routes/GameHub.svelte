<script lang="ts" >
	import { displayBoard, playerReady } from "$lib/stores";
	import { Button, Spinner } from "flowbite-svelte";
    const hubConnectBtn = () => {
        $playerReady = !$playerReady
    }

</script>

{#if !$displayBoard}

    <Button class="my-10" on:click={hubConnectBtn}>
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