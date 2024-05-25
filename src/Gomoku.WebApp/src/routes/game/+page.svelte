<script lang="ts">
    import type { PageData } from './$types';
	import GameHub from '../GameHub.svelte';
	import GameBoard from '../GameBoard.svelte';
	import { player } from '$lib/stores';
	import GamePanel from '../GamePanel.svelte';
    
    export let data: PageData;
	const getGames = (async () => {
		const response = await fetch('http://localhost:5190/Game')
		return await response.json();
	})()
</script>

<svelte:head>
	<title>About</title>
	<meta name="description" content="About this app" />
</svelte:head>
<div class="text-column">

	<!-- svelte-ignore empty-block -->
	{#await getGames}
	{:then games}
	{#if games.length != 0}
		<p>Games</p>
		<ul>
			{#each games as game}
				<li>Code: {game.code}, Black Name: {game.blackName}, White Name: {game.whiteName}</li>
			{/each}
		</ul>
	{/if}
	{:catch error}
		<p>An error occurred.</p>
	{/await}

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
