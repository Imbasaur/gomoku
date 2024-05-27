<script lang="ts">
    import Clock from "$lib/components/Clock.svelte";     
    import { clock, displayBoard } from "$lib/stores";
</script>


{#if $displayBoard == true}
    <div id="panel">
        <div>
            {#if $clock != null}
                <p>Black {$clock.black}</p>
                <Clock color = 'black'/>
                <p>White {$clock.white}</p>
                <Clock color = 'white'/>
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