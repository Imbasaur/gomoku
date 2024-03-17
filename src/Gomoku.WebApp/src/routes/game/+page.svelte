<script lang="ts">
    import type { PageData } from './$types';
	import GameHub from '../GameHub.svelte';
    
    export let data: PageData;
</script>

<svelte:head>
	<title>About</title>
	<meta name="description" content="About this app" />
</svelte:head>

<div class="text-column">

<GameHub />
</div>



