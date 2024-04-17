<script lang="ts">
    import type { PageData } from './$types';
	import GameHub from '../GameHub.svelte';
    
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
	

<div id="board">
	{#each Array.from(Array(3).keys()) as row (row)}
		<div class="row{row}">
			{#each Array.from(Array(3).keys()) as column (column)}
				<div class="node" id="node{row}{column}">x</div>
			{/each}
		</div>
	{/each}
</div>
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
	<GameHub />
</div>

<style>
	#board {
		margin-left: auto;
		margin-right: auto;
		width: 150px;
		height: 150px;
		display: grid;
		grid-template-columns: repeat(3, 1fr);
		grid-gap: 10px;
	}

	.node {
		width: 10px;
		height: 10px;
		border: 1px solid #D3D3D3;
		background-color: #F5F5F5;
		font-size: 40px;
		display: flex;
		justify-content: center;
		align-items: center;
	}
</style>



