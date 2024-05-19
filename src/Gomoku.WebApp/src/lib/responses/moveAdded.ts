import type { Clock } from "$lib/types/Clock";

export interface MoveAdded {
    readonly move: string;
    readonly clock: Clock;
}