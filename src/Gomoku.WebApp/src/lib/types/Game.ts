export interface Game {
    readonly code: string;
    readonly moves: string;
    readonly state: number;
    readonly blackName: string;
    readonly whiteName: string;
    readonly Winner: string;
    readonly isWhiteConnected: boolean;
    readonly isBlackConnected: boolean;
    readonly variant: number;
}