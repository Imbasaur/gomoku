import { activePlayer, displayBoard, gameFinished, latestMove, moves, winningStones } from "$lib/stores";

export function clearBoard(){
    displayBoard.set(false)
    gameFinished.set(false)
    winningStones.set([])
    activePlayer.set('')
    latestMove.set('')
    moves.set([])
}