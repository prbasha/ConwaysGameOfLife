# Conway's Game Of Life
An implementation of Conway's Game of Life cellular automaton.

The game is represented by square grid of cells.

At any time, a single cell **CC** can be in one 2 states:
* Alive
* Dead

To update cell **C**, the number of living cells **N** in C's eight location neighbourhood is calculated.
**C** is then updated, accoridng to this table:
| C current status  | N | C new status |
| --- | --- | --- |
| Alive  | 0,1  | Dead (lonely) |
| Alive  | 4,5,6,7,8  | Dead (overcrowded) |
| Alive  | 2,3  | Alive (lives) |
| Dead  | 3  | 1 (takes 3 to give birth) |
| Dead  | 0,1,2,3,4,5,6,7,8  | Dead (barren) |

Any cells beyond the square grid are always dead.

For more information: https://rosettacode.org/wiki/Conway%27s_Game_of_Life
