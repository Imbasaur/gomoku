<script lang="ts" >
	import { activePlayer, afterGameModal, clock, displayBoard, gameFinished, gameInfo, gameWinner, latestMove, moves, playerReady, winningStones } from "$lib/stores";
	import { Button, Spinner } from "flowbite-svelte";
    const hubConnectBtn = () => {
        $playerReady = !$playerReady
        if ($displayBoard){
            $displayBoard = false
            $gameFinished = false
            $winningStones = []
            $clock = 
            $activePlayer = ''
            $latestMove = ''
            $gameInfo = null
            $moves = []
        }
    }

</script>

{#if !$displayBoard || (!$playerReady && !$afterGameModal)}

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