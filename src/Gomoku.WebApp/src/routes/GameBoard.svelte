<script lang="ts">
    import { HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
    import { onDestroy, createEventDispatcher } from 'svelte';
    import WaitingList from "./WaitingList.svelte";
    import joinWaitingList from './WaitingList.svelte'
    import { waitingList, player } from "$lib/stores";
	import Counter from "./Counter.svelte";

    let blackTurn = true

    function handleClick(column, row){
        console.log("Node" + column + row +" clicked by " + blackTurn ? 'black' : 'white' + " player.")
        document.getElementById("node" + column + row)?.classList.add(blackTurn ? 'black' : 'white')
        blackTurn = !blackTurn;
    }
</script>
	
<div id="board">
    {#each Array.from(Array(15).keys()) as column}
        <div class="column{column}">
            {#each Array.from(Array(15).keys()) as row}
            <div class="node node{column}{row} column-{column} row-{row}" id="node{column}{row}" on:click|once={() => handleClick(column, row)}></div>
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
	.node00::before,
	.node00::after {
		content: '';
		position: absolute;
		top: 15px;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	.node00::after {
		left: 15px;
		top: 15px;
		width: 50%;
		height: 1px;
	}

	/* Bottom left corner */
	#node014::before,
	#node014::after {
		content: '';
		position: absolute;
		top: 0;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	#node014::after {
		left: 15px;
		top: 15px;
		width: 50%;
		height: 1px;
	}

	/* Top right corner */
	#node140::before,
	#node140::after {
		content: '';
		position: absolute;
		top: 15px;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	#node140::after {
		left: 0;
		top: 15px;
		width: 50%;
		height: 1px;
	}

	/* Top right corner */
	#node1414::before,
	#node1414::after {
		content: '';
		position: absolute;
		top: 0;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	#node1414::after {
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
