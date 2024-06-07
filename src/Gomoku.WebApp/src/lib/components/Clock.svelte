<script>
    import { activePlayer, clock, gameFinished, gameInfo } from "$lib/stores";
    import { onDestroy, tick } from "svelte";
    import { get } from 'svelte/store';
    import { fade } from 'svelte/transition';

    export let color;

    function formatMs(ms) {
        let seconds = ms / 1000;
        let formattedSeconds = seconds.toFixed(2);
        return formattedSeconds;
    }
    
    let ms = 10;
    let countdown = color === 'black' ? get(clock).black : get(clock).white;
    let now = Date.now();
    let end = now + countdown * 1000;
    let difference = null;
    let showDifference = false;
    let differenceColor = '';
    let differenceSymbol = '';

    let interval;

    const unsubscribeClock = clock.subscribe(async ($clock) => {
        let newCountdown = color === 'black' ? $clock.black : $clock.white;
        let otherCountdown = color === 'black' ? $clock.white : $clock.black;
        let newCountdownMs = newCountdown * 1000;
        let oldCountdownMs = countdown * 1000;
        let diffMs = Math.abs(newCountdownMs - oldCountdownMs);

        let shouldShowDifference = !(newCountdown === $gameInfo.time && otherCountdown === $gameInfo.time) && diffMs !== 0 && !isNaN(diffMs);

        if (shouldShowDifference && diffMs !== undefined) {
            if (newCountdownMs < oldCountdownMs) {
                differenceColor = 'red';
                differenceSymbol = '-';
            } else {
                differenceColor = 'green';
                differenceSymbol = '+';
            }

            difference = differenceSymbol + formatMs(diffMs);

            showDifference = true;

            await tick();

            setTimeout(() => {
                showDifference = false;
            }, 100); 
        }

        countdown = newCountdown;
        now = Date.now();
        end = now + countdown * 1000;
    });

    const unsubscribeGameFinished = gameFinished.subscribe(($gameFinished) => {
        if ($gameFinished) clearInterval(interval);
    });

    const updateTimer = () => {
        if (color === get(activePlayer)) {
            now = Date.now();
            let remainingTime = end - now;
            if (remainingTime > 0) {
                countdown = formatMs(remainingTime);
            } else {
                countdown = '0.00';
                clearInterval(interval);
            }
        }
    };

    onDestroy(() => {
        clearInterval(interval);
        unsubscribeClock();
        unsubscribeGameFinished();
    });

    interval = setInterval(updateTimer, ms);
</script>

<div class="clock">
    <p>
        {countdown}
        {#if showDifference}
            <span class="difference" style="color: {differenceColor}" out:fade={{ delay: 1000, duration: 2000 }}>{difference}</span>
        {/if}
    </p>
</div>

<style>
    .difference {
        transition: opacity 1s ease-out;
    }
</style>
