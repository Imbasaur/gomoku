export interface Game {
    readonly Code: string;
    readonly Moves: string;
    readonly State: number;
    readonly BlackName: string;
    readonly WhiteName: string;
    readonly Winner: string;
    readonly IsWhiteConnected: boolean;
    readonly IsBlackConnected: boolean;
    readonly Variant: number;
}