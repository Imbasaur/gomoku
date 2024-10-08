export interface Game {
    readonly id: number;
    readonly code: string;
    readonly state: number;
    readonly blackName: string;
    readonly whiteName: string;
    readonly time: number;
    readonly variant: number;
}