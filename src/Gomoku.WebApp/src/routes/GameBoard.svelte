<script lang="ts">
    import { HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
    import { onDestroy, createEventDispatcher } from 'svelte';
    import WaitingList from "./WaitingList.svelte";
    import joinWaitingList from './WaitingList.svelte'
    import { waitingList, player } from "$lib/stores";
	import Counter from "./Counter.svelte";

    let blackTurn = true

    function handleClick(column:number, row:number){
        var color = blackTurn ? 'black' : 'white';
        console.log("Node" + column + "x" + row +" clicked by " + color + " player.")
        document.getElementById("node" + column + 'x' + row)?.classList.add(color)
        checkWin(column, row, color);

        blackTurn = !blackTurn;
    }

    function checkWin(column:number, row:number, color:string){
        console.log("Checking win conditions for " + color + " on " + column + 'x' + row);
        var fiveInARow = false
        for (let i = -1; i <= 1; i++) {
            for (let j = -1; j <= 1; j++) {
                if (i == 0 && j == 0)
                    continue;
                
                var count = 1 + checkDir(column, row, i, j, color)
                if (count > 1 && count < 5){
                    console.log("Found " + count + " stones, checking opposite direction")
                    count += checkDir(column, row, 0-i, 0-j, color)
                    break;
                }

                if (count >= 5)
                    fiveInARow = true;
            }
            if (fiveInARow)
                break;
        }
        
        if (fiveInARow){
            console.log("Found five in a row by " + color + "")
            alert(color + " won the game");
        }
    }

    function checkDir(column:number, row:number, x:number, y:number, color:string){
        let c = 0;
        console.log("Looking for " + color + " stone on " + (column + x) + "x" + (row + y));
        if ((column + x) >= 0 && (column + x) <= 14 && (row + y) >= 0 && (row + y) <= 14 && document.getElementById("node" + (column + x) + 'x' + (row + y))?.classList.contains(color)){
            console.log("Found next " + color + " stone on " + (column + x) + "x" + (row + y));
            c = 1 + checkDir(column + x, row + y, x, y, color);
        }
        return c
    }
</script>
	
<div id="board">
    {#each Array.from(Array(15).keys()) as column}
        <div class="column{column}">
            {#each Array.from(Array(15).keys()) as row}
            <div class="node node{column}x{row} column-{column} row-{row}" id="node{column}x{row}" on:click|once={() => handleClick(column, row)}></div>
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
	.column-0::after{
		content: '';
		position: absolute;
		top: 15px;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	.column-0::after{
			left: 15px;
			top: 15px;
			width: 50%;
			height: 1px;
	}

	/* Right border */ 
	.column-14::before{
		content: '';
		position: absolute;
		top: 0;
		left: 15px;
		width: 1px;
		height: 100%;
		background-color: #000000;
	}
	.column-14::after{
		left: 0;
		top: 15px;
		width: 50%;
		height: 1px;
	}

	/* Top border */ 
	.row-0::before{
		content: '';
		position: absolute;
		top: 15px;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	.row-0::after{
			left: 0;
			top: 15px;
			width: 100%;
			height: 1px;
	}

	/* Bottom border */ 
	.row-14::before{
		content: '';
		position: absolute;
		top: 0;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	.row-14::after{
		left: 0;
		top: 15px;
		width: 100%;
		height: 1px;
	}

    /* Top left corner */
	.node0x0::before,
	.node0x0::after {
		content: '';
		position: absolute;
		top: 15px;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	.node0x0::after {
		left: 15px;
		top: 15px;
		width: 50%;
		height: 1px;
	}

	/* Bottom left corner */
	#node0x14::before,
	#node0x14::after {
		content: '';
		position: absolute;
		top: 0;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	#node0x14::after {
		left: 15px;
		top: 15px;
		width: 50%;
		height: 1px;
	}

	/* Top right corner */
	#node14x0::before,
	#node14x0::after {
		content: '';
		position: absolute;
		top: 15px;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	#node14x0::after {
		left: 0;
		top: 15px;
		width: 50%;
		height: 1px;
	}

	/* Top right corner */
	#node14x14::before,
	#node14x14::after {
		content: '';
		position: absolute;
		top: 0;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	#node14x14::after {
		left: 0;
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
</style>
