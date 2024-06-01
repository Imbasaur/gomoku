<script lang="ts">
    import type { PageData } from './$types';
	import GameHub from '../GameHub.svelte';
	import GameBoard from '../GameBoard.svelte';
	import { player } from '$lib/stores';
	import GamePanel from '../GamePanel.svelte';
	import { onMount } from 'svelte';
	import type { Game } from '$lib/types/Game';
    
    export let data: PageData;
	let gamesList: Game[]

	function getGames(): Promise<Game[]> {
		return fetch('http://localhost:5190/Game')
			.then(response => response.json())
			.then( response => {
				return response as Game[]
			})
	}

	onMount(() => {
		getGames()
			.then(games => gamesList = games)
	})
</script>

<svelte:head>
	<title>About</title>
	<meta name="description" content="About this app" />
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

	{#if $player != ''}
	<div>
		<p>Player name is {$player}</p>
	</div>
	{/if}

	<GameHub />
	<div class="game-panel">
		<GameBoard />
		<GamePanel />
	</div>
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
