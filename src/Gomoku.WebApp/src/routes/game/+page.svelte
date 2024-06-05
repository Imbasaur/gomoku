<script lang="ts">
    import type { PageData } from './$types';
	import GameHub from '../GameHub.svelte';
	import GameBoard from '../GameBoard.svelte';
	import { activePlayer, afterGameModal, clock, displayBoard, gameFinished, gameInfo, gameWinner, latestMove, moves, player, playerReady, winningStones } from '$lib/stores';
	import GamePanel from '../GamePanel.svelte';
	import { onMount } from 'svelte';
	import type { Game } from '$lib/types/Game';
	import { PUBLIC_BACKEND_ADDRESS } from '$env/static/public';
  	import { Button, Modal } from 'flowbite-svelte';
    
    export let data: PageData;
	let gamesList: Game[]

	gameFinished.subscribe((value) => {
		if (value && !$afterGameModal){
			$afterGameModal = true
		}
	})

	function getGames(): Promise<Game[]> {
		return fetch(PUBLIC_BACKEND_ADDRESS + '/Game')
			.then(response => response.json())
			.then( response => {
				return response as Game[]
			})	
	}
    const playNextBtn = () => {
            $displayBoard = false
			$playerReady = true
            $gameFinished = false
            $winningStones = []
            $clock = 
            $activePlayer = ''
            $latestMove = ''
            $gameInfo = null
            $moves = []
    }

    const finishPlayingBtn = () => {
		// todo: show find game btn?
    }

	onMount(() => {
		// getGames()
		// 	.then(games => gamesList = games)
	})
</script>

<svelte:head>
	<title>Gomoku</title>
	<meta name="description" content="Gomoku game" />
</svelte:head>
<div class="text-column">

	<!-- {#if gamesList != undefined && gamesList.length != 0}
		<p>Games</p>
		<ul>
			{#each gamesList as game}
				<li>Code: {game.code}, Black Name: {game.blackName}, White Name: {game.whiteName}</li>
			{/each}
		</ul>
	{/if} -->

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
