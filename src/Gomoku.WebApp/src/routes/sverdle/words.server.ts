/** The list of possible words */
export const words = [
	'aback'
];

/** The list of valid guesses, of which the list of possible words is a subset */
export const allowed = new Set([
	...words,
	'aahed',
]);
