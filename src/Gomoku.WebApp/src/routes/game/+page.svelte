<script lang="ts">
    import type { PageData } from './$types';
	import GameHub from '../GameHub.svelte';
	import GameBoard from '../GameBoard.svelte';
	import { activePlayer, afterGameModal, displayBoard, gameFinished, gameWinner, player, playerReady } from '$lib/stores';
	import GamePanel from '../GamePanel.svelte';
  	import { Button, Modal } from 'flowbite-svelte';
	import { clearBoard, removeParamFromUrl } from '$lib/utils';
	import { joinWaitingList } from '$lib/gameActions';
	import { onDestroy } from 'svelte';
    
    export let data: PageData;

	gameFinished.subscribe((value) => {
		if (value && !$afterGameModal){
			$afterGameModal = true
		}
	})

    const playNextBtn = () => {
		clearBoard();
		$playerReady = true;
		joinWaitingList();
		removeParamFromUrl('id');
    }

    const finishPlayingBtn = () => {
		$afterGameModal = false
		$activePlayer = "";
		gameFinished.set(false)
    }

	onDestroy(async () => {
		finishPlayingBtn();
	})
</script>

<svelte:head>
	<title>Gomoku</title>
	<meta name="description" content="Gomoku game" />
</svelte:head>

<div class="text-column">
	{#if $player != '' && !$displayBoard}
	<div>
		<p>
			Welcome <b>{$player}</b>!<br>
			{#if !$playerReady}
				Press button below to start searching for a game
			{/if}
		</p>
	</div>
	{/if}
	
	<GameHub />
	<div class="game-panel">
		<GameBoard />
		<GamePanel />
	</div>

    <Modal bind:open={$afterGameModal} size="xs" autoclose outsideclose>
        <div class="text-center">
			<h2>{$gameWinner} won the game!</h2>			
			<h3 class="mb-5 text-lg font-normal text-gray-500 dark:text-gray-400">Fancy playing another one?</h3>
			<Button class="me-2" on:click={playNextBtn}>Yep</Button>
			<Button color="alternative" on:click={finishPlayingBtn}>No, let me go</Button>
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
</style>
