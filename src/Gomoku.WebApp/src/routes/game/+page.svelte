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
	

<div class="grid">
	{#each Array.from(Array(6).keys()) as row (row)}
		<div class="row">
			{#each Array.from(Array(5).keys()) as column (column)}
				<div class="node">
					<input name="guess" type="hidden" />
				</div>
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



