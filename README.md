# Unity Automated Testing Project

[Watch the showcase video on YouTube](https://www.youtube.com/watch?v=Bcl44QiA1H8)

This project was created as part of a technical assessment to demonstrate automated playmode testing in Unity, using the [Endless Runner Sample Game](https://assetstore.unity.com/packages/templates/tutorials/endless-runner-sample-game-87901) as the foundation.

**Code Coverage** is used to report current tests scenario.

![Test Coverage](./docs/CodeCoverage/Report/badge_linecoverage.svg)


## Tested Features

### Main required tests
- **Character Boundaries:** A test to ensure that repeated left-clicks do not cause the character to move off-screen.
- **Scoring by Collecting Fish:** Verifies that the player gains points upon collecting a fish.
- **Player die after hitting something:** If the player has 1 HP, it dies when hitting something.
- **Reset process working as expected:** Ensures that when the player collides with an obstacle (e.g., sand piles) and dies, the game resets correctly when "Play Again" is pressed.

### Extra tests
- **Consumables:** Verify if the consumables modifiers and powerups behaves as expected during gameplay.
- **Main Menu:** Testing UI interactions with options on menu such as Player Data Deletion and Quit/Start buttons are behaving as expected.
- **Shop purchases:** Ensure that the purchase of items in the game are behaving as expected.
- **EditMode (Unit Tests):** Implemented some specific behaviours of Consumables and Missions classes implementation.
- **General tests:** Following the Code Coverage results, some other tests were implemented.

## Implementation Details

- **No Game Code Changes:** The original game code was left largely unchanged. Additional scripts were created to manage the test scenes and logic.
- **Assembly Definition:** An assembly definition file (`Game.asmdef`) was added to the `Scripts` folder to better organize and compile the test framework.
- **Test Scene Management:** Instead of using existing game scenes, the tests dynamically load and unload scenes using `SetUp` and `TearDown` methods.
- **Simulated Input:** User input, such as clicks, was simulated in tests to mimic actual player behavior and ensure functional accuracy.
- **Tests runs at 2x game speed:** Ensures that the game's logic is considering framerate variations that occur during gameplay, and everything behaves as expected.

## Considerations and Future Improvements

- **Input System:** The current implementation uses the old Unity input system. A potential improvement would be to upgrade to the Unity Input System for more robust and modern input handling.
- **Code Coverage:** Current Code Coverage is over 60%, future implementation would consider 100% code coverage with test variations of current tested methods.

## Prerequisites

- **Unity Asset:** The [Endless Runner Sample Game](https://assetstore.unity.com/packages/templates/tutorials/endless-runner-sample-game-87901).
- **Unity Version:** 2022.3.40f1 or newer

## How to Run the Tests

1. Clone the repository to your local machine.
2. Open the project in Unity.
3. Use the Unity Test Runner in the Editor to run the Playmode tests. There are also Editmode tests implemented.
4. Verify that all tests pass and the game behaves as expected.
