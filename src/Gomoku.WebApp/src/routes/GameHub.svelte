<script lang="ts" >
	import { displayBoard, playerReady } from "$lib/stores";

    function triggerPlayerReady() {
        $playerReady = !$playerReady;
    };
</script>

{#if !$displayBoard}
    <button id="hub-button" on:click|preventDefault={triggerPlayerReady}>
        {#if $playerReady}
            Cancel
        {:else}
            Find game
        {/if}
    </button>
    
    {#if $playerReady}
        <p>Searching for an opponent</p>
    {/if}
{/if}

<style>
    #hub-button{
        margin-top: 10px;
        margin-bottom: 10px;
    }
</style>