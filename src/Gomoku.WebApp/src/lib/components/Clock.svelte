<script>
	import { activePlayer, clock } from "$lib/stores";
	import { onDestroy } from "svelte";

    export let color
    
    function fromatMs(ms) {
        let seconds = ms / 1000;
        let formattedSeconds = seconds.toFixed(2);
        return formattedSeconds;
    }

    let countdown =  (color == 'black' ? $clock.black : $clock.white)
    let ms = 1000
    let now = Date.now();
    let end = now + countdown * 1000
    $: {
        countdown = (color == 'black' ? $clock.black : $clock.white)
        //end = now + countdown * 1000
    }

    const updateTimer = () => {
        if (color == $activePlayer){
            let val = fromatMs(end-now)
            console.log('color: ' + color + ', countdown: ' + countdown + ', newTimeLeft: ' + val)
            //now = Date.now();
            if (val > 0){
                countdown = val
            }
            else {
                countdown = 0
                // call backend
            }

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