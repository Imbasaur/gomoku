<script lang="ts">
    import Clock from "$lib/components/Clock.svelte";
     
    import { clock, displayBoard } from "$lib/stores";


</script>


{#if $displayBoard == true}
    <div id="panel">
        <div>
            {console.log($clock)}
            {#if $clock != null}
                <p>Black</p>
                <Clock bind:countdown={$clock.black}/>
                <p>White</p>
                <Clock bind:countdown={$clock.white}/>
            {/if}
        </div>
    </div>
{/if}

<style>
    #panel {
        height: 600 px;
        width: 300px;
        background-color: #e3bc88;
    }
</style>