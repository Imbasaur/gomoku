<script>
	import { activePlayer } from "$lib/stores";
	import { onDestroy } from "svelte";



    export let countdown
    export let color
    
    function fromatMs(ms) {
        let seconds = ms / 1000;
        let formattedSeconds = seconds.toFixed(2);
        return formattedSeconds;
    }

    let ms = 100
    let now = Date.now();
    let end = now + countdown * 1000

    const updateTimer = () => {
        if (color == $activePlayer){
            let val = fromatMs(end-now)
            now = Date.now();
            // let val = Math.round((countdown - (ms/1000)) * 100) / 100
            if (val > 0){
                countdown = val
                return
            }

            countdown = 0
            // call backend
        }
    }

    let interval = setInterval(updateTimer, ms)
    $: if (countdown == 0)
        clearInterval(interval)

    onDestroy(() => {
        clearInterval(interval);
    });
</script>

<p>
    {countdown}
</p>


<style>

</style>