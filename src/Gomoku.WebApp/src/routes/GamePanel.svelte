<script lang="ts">
    import Clock from "$lib/components/Clock.svelte";     
	import { GameVariant } from "$lib/enums/GameVariant";
    import { clock, displayBoard, gameInfo, player } from "$lib/stores";
</script>


{#if $displayBoard == true}
    <div id="panel">
        <div class="game-info">
            <p>Variant: {GameVariant[$gameInfo.variant]}</p>
            <p>Time: {$gameInfo.time} seconds</p>
            <p>Black starts<b>{$gameInfo.blackName == $player ? ' (YOU)' : ''}</b></p>
        </div>
        <div class="player-info black">
            <p>{$gameInfo.blackName}<b>{$gameInfo.blackName == $player ? ' (YOU)' : ''}</b></p>
            {#if $clock != null}
                <Clock color = 'black'/>
            {:else}
                <p>{$gameInfo.time}</p>
            {/if}
        </div>

        <div class="player-info white">
            <p>{$gameInfo.whiteName}<b>{$gameInfo.whiteName == $player ? ' (YOU)' : ''}</b></p>
            {#if $clock != null}
                <Clock color = 'white'/>
            {:else}
                <p>{$gameInfo.time}</p>
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

    .game-info {
        margin: 5px;
        padding: 0 5px 5px 10px;
    }

    .player-info {
        border-radius: 15px;
        border-style: solid;
        margin: 5px;
        padding: 0 5px 5px 10px;
    }

    #panel > .black {
        border-color: #262421;
        background-color: #302E2C;
        color: #BABABA;
    }

    #panel > .white {
        border-color: white;
        background-color: #DCDCDC;
        color: black;
    }
</style>