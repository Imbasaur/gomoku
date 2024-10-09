import { activePlayer, displayBoard, gameFinished, latestMove, moves, winningStones } from "$lib/stores";

export function clearBoard(){
    displayBoard.set(false)
    gameFinished.set(false)
    winningStones.set([])
    activePlayer.set('')
    latestMove.set('')
    moves.set([])
}

export function addParamToUrl(key: string, value: string){
    const currentUrl = new URL(window.location.href);
    currentUrl.searchParams.set(key, value);
    window.history.pushState({}, '', currentUrl);
}

export function removeParamFromUrl(key: string){
    const currentUrl = new URL(window.location.href);
    currentUrl.searchParams.delete(key);
    window.history.pushState({}, '', currentUrl);
}