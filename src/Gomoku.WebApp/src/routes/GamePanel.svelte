<script lang="ts">
    import Clock from "$lib/components/Clock.svelte";
     
    import { clock, displayBoard } from "$lib/stores";


</script>


{#if $displayBoard == true}
    <div id="panel">
        <div>
            {#if $clock != null}
                <p>Black</p>
                <Clock bind:countdown={$clock.black} color = 'black'/>
                <p>White</p>
                <Clock bind:countdown={$clock.white} color = 'white'/>
            {/if}
        </div>
    </div>
{/if}

<style>
    #panel {
        height: 450px;
        width: 250px;
        background-color: #e3bc88;
    }
</style>