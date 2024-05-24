<script>

    export let countdown

    let ms = 100
    const decr = () => {
        let val = Math.round((countdown - (ms/1000)) * 100) / 100
        if (val > 0){
            countdown = val
            return
        }

        countdown = 0
        // call backend

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