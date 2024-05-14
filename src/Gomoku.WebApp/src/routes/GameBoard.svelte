<script lang="ts">
	import { gameCode, moves } from "$lib/stores";

    function handleClick(column:number, row:number){
        addMove(numToAlpha(column) + row);
        console.log("Clientside - Node " + numToAlpha(column) + row )
    }
    
    function addMove(move: string) {  // this should be signalr call
        console.log("code is " + $gameCode);
        fetch("http://localhost:5190/Game/move", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ code: $gameCode, move})
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to add a move to the game')
            }
            console.log("Successfully added move the game.");
            // anything else?
        })
        .catch(error => {
            console.error('Error adding move to the game:', error.message);
        });
    }

    function numToAlpha(n: number){
        return String.fromCharCode(96 + n)
    }

    function getColor(index: number){
        if (index % 2 == 1)
            return "black"
        return "white"
    }
    
    function addStone(node: string, index: number){
        console.log('Adding stone on ' + node)
        document.getElementById("node-" + node)?.classList.add(getColor(index))
    }

    $: if ($moves.length > 0)
        addStone($moves[$moves.length - 1], $moves.length)

</script>
<div id="board">
    {#each Array.from(Array(15).keys()) as column}
        <div class="{numToAlpha(column+1)}">
            {#each Array.from(Array(15).keys()) as row}
                <div class="node {numToAlpha(column+1)}{15-row} node-{numToAlpha(column+1)}{15-row} column-{numToAlpha(column+1)} row-{15-row}" id="node-{numToAlpha(column+1)}{15-row}" on:click|once={() => handleClick(1+column, 15-row)}></div>
            {/each}
        </div>
    {/each}
</div>
<style>
    #board {
        margin-left: auto;
        margin-right: auto;
        width: 600px;
        height: 600px;
        display: grid;
        grid-template-columns: repeat(15, 0fr);
    }

    .node {
        width: 30px;
        height: 30px;
        position: relative; /* Add position relative */
        background-color: #ECB163;
        font-size: 40px;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    /* Horizontal line */
    .node::after {
        content: '';
        position: absolute;
        top: 15px; /* Position in the middle vertically */
        left: 0;
        width: 100%;
        height: 1px; /* Height of the line */
        background-color: #000000;
    }

    /* Vertical line */
    .node::before {
        content: '';
        position: absolute;
        top: 0;
        left: 15px; /* Position in the middle horizontally */
        width: 1px; /* Width of the line */
        height: 100%;
        background-color: #000000;
    }

	/* Left border */ 
	.column-a::after{
		content: '';
		position: absolute;
		top: 15px;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	.column-a::after{
			left: 15px;
			top: 15px;
			width: 50%;
			height: 1px;
	}

	/* Right border */ 
	.column-o::before{
		content: '';
		position: absolute;
		top: 0;
		left: 15px;
		width: 1px;
		height: 100%;
		background-color: #000000;
	}
	.column-o::after{
		left: 0;
		top: 15px;
		width: 50%;
		height: 1px;
	}

	/* Top border */ 
	.row-15::before{
		content: '';
		position: absolute;
		top: 15px;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	.row-15::after{
			left: 0;
			top: 15px;
			width: 100%;
			height: 1px;
	}

	/* Bottom border */ 
	.row-1::before{
		content: '';
		position: absolute;
		top: 0;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	.row-1::after{
		left: 0;
		top: 15px;
		width: 100%;
		height: 1px;
	}

    /* Top left corner */
	.node-a15::before,
	.node-a15::after {
		content: '';
		position: absolute;
		top: 15px;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	.node-a15::after {
		left: 15px;
		top: 15px;
		width: 50%;
		height: 1px;
	}

	/* Top right corner */
	#node-o15::before,
	#node-o15::after {
		content: '';
		position: absolute;
		top: 15px;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	#node-o15::after {
		left: 0;
		top: 15px;
		width: 50%;
		height: 1px;
	}

	/* Bottom right corner */
	#node-o1::before,
	#node-o1::after {
		content: '';
		position: absolute;
		top: 0;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	#node-o1::after {
		left: 0;
		top: 15px;
		width: 50%;
		height: 1px;
	}

    /* Bottom left corner */
    #node-a1::before,
    #node-a1::after {
        content: '';
        position: absolute;
        top: 0;
        left: 15px;
        width: 1px;
        height: 50%;
        background-color: #000000;
    }
    #node-a1::after {
        left: 15px;
        top: 15px;
        width: 50%;
        height: 1px;
    }

    .black::after {
        left: 3px;
        top: 3px;
        height: 24px;
        width: 24px;
        background-color: black;
        border-radius: 50%;
        display: inline-block;
    }

    .white::after {
        left: 3px;
        top: 3px;
        height: 24px;
        width: 24px;
        background-color: white;
        border-radius: 50%;
        display: inline-block;
    }

    .test::after {
        left: 3px;
        top: 3px;
        height: 24px;
        width: 24px;
        background-color: rgb(255, 0, 0);
        border-radius: 50%;
        display: inline-block;
    }
</style>
