import { writable, readable } from "svelte/store";

export const waitingList = writable([]);



export const backendUrl = readable("http://localhost:5190/WaitingList");