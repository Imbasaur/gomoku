<script lang="ts">
    import { clock, displayBoard } from "$lib/stores";
</script>


{#if $displayBoard == true}
    <div id="panel">
        <div>
            {#if $clock != null}
                <p>Black {$clock.black}s left</p>
                <p>White {$clock.white}s left</p>
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