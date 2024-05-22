<script lang="ts" >
	import { playerReady } from "$lib/stores";

    function triggerPlayerReady() {
        $playerReady = !$playerReady;
    };
</script>

<button on:click|preventDefault={triggerPlayerReady}>
    {#if $playerReady}
        Cancel
    {:else}
        Start
    {/if}
</button>