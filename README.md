# Chess
This is a demo to show how simple drag-and-drop in Blazor opens up new oppurtunites to create user friendly interfaces.
## Code
By implementing a basic chess engine I ensure that all moves are valid. The chess engine is a state machine that keeps track of all moves.
## Design
Normal components are defined to handle: board, squares, pieces, timer, history, etc.
Standard Javascript events are used to handle drag-and-drop. They are caught by normal Blazor code and communicated between the components.
![Chess](chess.png?raw=true "Chess")

