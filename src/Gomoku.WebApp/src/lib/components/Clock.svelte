<script>
	import { activePlayer } from "$lib/stores";



    export let countdown
    export let color

    let ms = 100
    const decr = () => {
        if (color == $activePlayer){
            let val = Math.round((countdown - (ms/1000)) * 100) / 100
            if (val > 0){
                countdown = val
                return
            }

            countdown = 0
            // call backend
        }
    }

    let clear

    $: {
        clearInterval(clear)
        clear = setInterval(decr, ms)
    }

</script>

<p>
    {countdown}
</p>


<style>

</style>