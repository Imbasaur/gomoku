<script lang="ts">
    import { sendMove } from "$lib/gameActions";
    import { moves, displayBoard, winningStones, gameInfo } from "$lib/stores";

    function handleClick(e: any, column: number, row: number) {
        if (e.currentTarget.getAttribute('disabled') == null) {
            sendMove($gameInfo.code, numToAlpha(column) + row);
        }
    }

    function numToAlpha(n: number) {
        return String.fromCharCode(96 + n);
    }

    function getStoneColor(move: string) {
        const index = $moves.indexOf(move);
        return index !== -1 ? (index % 2 === 0 ? "black" : "white") : "";
    }

    function isWinningStone(node: string) {
        return $winningStones.includes(node);
    }
</script>

{#if $displayBoard}
    <div id="board">
        {#each Array.from(Array(15).keys()) as column}
            <div class="{numToAlpha(column + 1)}">
                {#each Array.from(Array(15).keys()) as row}
                    {#if $moves.includes(numToAlpha(column + 1) + (15 - row))}
                        <div class="node {numToAlpha(column + 1)}{15 - row} node-{numToAlpha(column + 1)}{15 - row} column-{numToAlpha(column + 1)} row-{15 - row}" 
                             id="node-{numToAlpha(column + 1)}{15 - row}" 
                             on:click|preventDefault={(e) => handleClick(e, 1 + column, 15 - row)}>
                            <div class="stone {getStoneColor(numToAlpha(column + 1) + (15 - row))} {isWinningStone(numToAlpha(column + 1) + (15 - row)) ? 'border' : ''}"></div>
                        </div>
                    {:else}
                        <div class="node {numToAlpha(column + 1)}{15 - row} node-{numToAlpha(column + 1)}{15 - row} column-{numToAlpha(column + 1)} row-{15 - row}" 
                             id="node-{numToAlpha(column + 1)}{15 - row}" 
                             on:click|preventDefault={(e) => handleClick(e, 1 + column, 15 - row)}>
                        </div>
                    {/if}
                {/each}
            </div>
        {/each}
    </div>
{/if}

<style>
    #board {
        margin-left: auto;
        margin-right: auto;
        width: 450px;
        height: 450px;
        display: grid;
        grid-template-columns: repeat(15, 0fr);
    }

    .node {
        width: 30px;
        height: 30px;
        position: relative;
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
        top: 15px;
        left: 0;
        width: 100%;
        height: 1px;
        background-color: #000000;
        z-index: 1;
    }

    /* Vertical line */
    .node::before {
        content: '';
        position: absolute;
        top: 0;
        left: 15px;
        width: 1px;
        height: 100%;
        background-color: #000000;
        z-index: 1;
    }

	.column-a::after {
		content: '';
		position: absolute;
		top: 15px;
		left: 15px;
		width: 1px;
		height: 50%;
		background-color: #000000;
	}
	.column-a::after {
			left: 15px;
			top: 15px;
			width: 50%;
			height: 1px;
	}

    /* Right border */ 
    .column-o::before {
        content: '';
        position: absolute;
        top: 0;
        left: 15px;
        width: 1px;
        height: 100%;
        background-color: #000000;
    }

    .column-o::after {
        content: '';
        position: absolute;
        left: 0;
        top: 15px;
        width: 50%;
        height: 1px;
        background-color: #000000;
    }

    /* Top border */ 
    .row-15::before {
        content: '';
        position: absolute;
        top: 15px;
        left: 15px;
        width: 1px;
        height: 50%;
        background-color: #000000;
    }

    .row-15::after {
        content: '';
        position: absolute;
        left: 0;
        top: 15px;
        width: 100%;
        height: 1px;
        background-color: #000000;
    }

    /* Bottom border */ 
    .row-1::before {
        content: '';
        position: absolute;
        top: 0;
        left: 15px;
        width: 1px;
        height: 50%;
        background-color: #000000;
    }

    .row-1::after {
        content: '';
        position: absolute;
        left: 0;
        top: 15px;
        width: 100%;
        height: 1px;
        background-color: #000000;
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

    .stone {
        position: absolute;
        left: 3px !important;
        top: 3px !important;
        width: 24px !important;
        height: 24px !important;
        border-radius: 50%;
        display: inline-block;
        background-color: transparent;
        z-index: 2;
    }

    .stone.black {
        background-color: black !important;
    }

    .stone.white {
        background-color: white !important;
    }

    .stone-border {
        border: 2px solid red;
    }
</style>
