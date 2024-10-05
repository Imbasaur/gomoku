<script lang="ts">
	import { activeGames, activePlayer, afterGameModal, displayBoard, gameFinished, gameWinner, player } from '$lib/stores';
	import { onMount } from 'svelte';
    import type { PageData } from './$types';
    import { Button, Modal } from 'flowbite-svelte';
	import { joinGame, subscribe } from '$lib/gameActions';
    import { EyeSolid } from 'flowbite-svelte-icons';
	import GameBoard from '../GameBoard.svelte';
	import GamePanel from '../GamePanel.svelte';
	import { clearBoard } from '$lib/utils';

    export let data: PageData;

    //let activeGames: string | any[] | undefined = [];

    onMount(async () => {
        subscribe("watchlist");
    });

    gameFinished.subscribe((value) => {
        if (value && !$afterGameModal && $displayBoard){
            $afterGameModal = true
        }
    })

    function handleWatchButtonClick(code: string) {
        joinGame(code, $player, true)
    }

    function okButtonClick() {
        $activePlayer = "";
        $afterGameModal = false;
        $gameFinished = false;
    }

    function watchAnotherButtonClick() {
        clearBoard();
        okButtonClick();
    }
</script>

<svelte:head>
	<title>Gomoku</title>
	<meta name="description" content="Gomoku game" />
</svelte:head>

<div class="text-column">

    {#if !$displayBoard}
        {#if $activeGames != undefined && $activeGames.length != 0}
            <ul>
                {#each $activeGames as game}
                <li>
                    <Button size="sm" on:click={() => handleWatchButtonClick(game.code)}>
                        <EyeSolid class="w-4 h-4 me-2" />Watch
                    </Button> 
                    {game.blackName} vs {game.whiteName}
                </li>
                {/each}
            </ul>
        {:else}
            <p>Currently there are no active games to watch</p>
            
        {/if}
    {/if}
    {#if $displayBoard && $activePlayer == ""}
        <Button class="my-10" on:click={watchAnotherButtonClick}>
            Watch another
        </Button>
    {/if}
    <div class="game-panel">
        <GameBoard />
        <GamePanel />
    </div>
    
    <Modal bind:open={$afterGameModal} size="xs" autoclose outsideclose>
        <div class="text-center">
			<h3>{$gameWinner} won the game!</h3>
			<Button color="alternative" on:click={okButtonClick}>Ok</Button>
			<Button class="me-2" on:click={watchAnotherButtonClick}>Watch another</Button>
        </div>
    </Modal>
</div>

<style>
	.game-panel {
		display: flex;
		max-width: 48rem;
		flex: 0.6;
		flex-direction: row;
		justify-content: center;
		margin: 0 auto;
	}
    li {
        padding-bottom: 2px;
    }
</style>
