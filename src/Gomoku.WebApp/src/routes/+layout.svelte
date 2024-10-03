<script lang="ts">
	import { createHubConnection, stopHubConnection } from '$lib/gameHub';
	import '../app.css';
	import Header from './Header.svelte';
	import './styles.css';
	import { onDestroy, onMount } from 'svelte';

	onMount(() => {
    createHubConnection();

    const handleBeforeUnload = () => {
        stopHubConnection();
    };

    window.addEventListener('beforeunload', handleBeforeUnload);

    onDestroy(() => {
        window.removeEventListener('beforeunload', handleBeforeUnload);
    });
});
</script>

<div class="app">
	<Header></Header>

	<main>
		<slot />
	</main>

	<footer>
		<p>visit <a href="https://kit.svelte.dev">kit.svelte.dev</a> to learn SvelteKit</p>
	</footer>
</div>

<style>
	.app {
		display: flex;
		flex-direction: column;
		min-height: 100vh;
	}

	main {
		flex: 1;
		display: flex;
		flex-direction: column;
		padding: 1rem;
		width: 100%;
		max-width: 64rem;
		margin: 0 auto;
		box-sizing: border-box;
	}

	footer {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		padding: 12px;
	}

	footer a {
		font-weight: bold;
	}

	@media (min-width: 480px) {
		footer {
			padding: 12px 0;
		}
	}
</style>
