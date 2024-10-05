import type { Clock } from "./Clock";

export interface InitGame {
    moves: string;
    blackName: string; 
    whiteName: string;
    time: number;
    readonly variant: number;
    clock: Clock;
}
