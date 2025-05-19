## Game functionality

### 1.User Interface (UI)
 Start Screen:
 * Log In to Profile
 * Register
![Start Screen](https://github.com/dayuun1/Saper-KPZ/blob/main/Saper/Images/11.jpg)
 Main Menu:
 * Start Game
 * View Game History
 * Change Profile
 * Log Out
![Main Menu](https://github.com/dayuun1/Saper-KPZ/blob/main/Saper/Images/12.jpg)
 Game Screen:
 * Display of a closed field with mines
 * Display of the current score
 * "Safe Move" button
 * Notification of victory or defeat
![Game Screen](https://github.com/dayuun1/Saper-KPZ/blob/main/Saper/Images/15.jpg)
 Game End Screen:
 * Display of the game result
 * "Play Again" button
 Escape Screen:
 * "Continue Game" button
 * "Exit to Menu" button
![Game Screen](https://github.com/dayuun1/Saper-KPZ/blob/main/Saper/Images/16.jpg)
### 2. Game logic
 Starting the game:
 * Field generation
 * Displaying side cells
 Player's game:
 * Ability to open the cell
 * Ability to safe check the cell
 * Ability to зlace the flag on the cell
 * Calculating the sum of points
 Victory check:
 * Determining the winner according to the classic rules of minesweeper
 * Announcing the result
### 3. Saving data
 Local database:
 * Saving a user profile
 * Saving game history
 * Saving information about each game (victory or defeat, score, difficulty, time spent)
 * Automatically saving information about the game after it ends
![Database](https://github.com/dayuun1/Saper-KPZ/blob/main/Saper/Images/13.jpg)
### 4. Additional features
 * Different difficulty levels (easy, medium, difficult)
 * Ability to choose the size of the playing field
![features](https://github.com/dayuun1/Saper-KPZ/blob/main/Saper/Images/14.jpg)

### Design Patterns
#### [Strategy Pattern](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/Models/Minefield.cs#17-58) - I use this pattern to generate a field.
#### [State Pattern](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/Models/DifficultyState/BeginnerState.cs#10-48) - I use this pattern to work with game difficulty levels to allow objects to change their behavior depending on its current state.
#### [Command Pattern](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/ViewModels/GameViewModel.cs#25-31) - I use this pattern to provide communication between the Model and the ViewModel.

### Programming Principles
 * [DRY](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/Models/Minefield.cs#54-57)
 * YAGNI (You Aren’t Gonna Need It) — the project does not contain redundant code: all classes, methods, and functions have a specific purpose and are actually used.
 * [Single Responsibility Principle](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/Models/Minefield.cs#60-97) 
 * [Open/Closed Principle](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/Models/DifficultyState/BeginnerState.cs#10)
 * [Dependency Inversion Principle](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/Models/Minefield.cs#17-22)
 * [Interface Segregation Principle](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/Models/DifficultyState/IDifficultyState.cs#9-14)

### Refactoring Techniques
 * [Replace Magic Number](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/Models/DifficultyState/HardState.cs#13)
 * [Extract Interface](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/Models/DifficultyState/IDifficultyState.cs#9-14)
 * [Encapsulate Field](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/ViewModels/MenuViewModel.cs#26-67)
 * [Extract Method](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/Models/Minefield.cs#99-119)
 * [Extract Class](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/Models/Minefield.cs#60-69)
 * [Replace Conditional with Polymorphism](https://github.com/dayuun1/Saper-KPZ/blob/readmeFile/Saper/Models/DifficultyState/BeginnerState.cs#31)

### How to launch
 * Clone the repository to your computer 
 * Make sure you have the correct version of the .NET SDK installed to run the project.
 * Make sure you have SQL Server installed and running locally to work with the database.
 * Open the project in the command line or in the IDE.
 * Run the project via the dotnet run command or the run button in the IDE.

### Number of lines
![Number of lines](https://github.com/dayuun1/Saper-KPZ/blob/main/Saper/Images/rows.png)



