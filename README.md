# Unity Automated Testing Project
![Test Coverage](./CodeCoverage/Report/badge_linecoverage.png)

This project was developed as part of a technical assignment to demonstrate automated testing in Unity, using the **Endless Runner Sample Game (Trash Dash)** as the basis.

## Tested Features

- **Character Movement:** A test to ensure that the character does not move off-screen when clicking left multiple times.
- **Gaining Points by Collecting Fish:** Verifies if the player gains the correct amount of points upon collecting a fish.
- **Reset After Collisions:** Ensures that after colliding with an obstacle (e.g., sand piles), the game resets correctly and the player can start again.

## Implementation Details

- **No Modifications to Game Code:** The original game code was not altered in any way.
- **Assembly Definition:** An assembly definition file, `Game.asmdef`, was added to the `Scripts` folder to organize the test framework.
- **Test Setup and Teardown:** Tests do not rely on separate scenes inside the project folders. Instead, they use dynamic `SetUp` and `TearDown` methods to load and unload scenes as needed for each test.
- **Input Handling:** Instead of simulating input, the tests directly call the methods triggered by user input, ensuring accuracy in behavior testing.

## Limitations and Suggestions

- **Input System:** The tests were built using the old input system, even though I am aware the new Unity Input System could have been implemented. This would be a suggested improvement to modernize the input handling in future iterations of the project.

## Prerequisites

- **Unity Asset:** The [Endless Runner Sample Game](https://assetstore.unity.com/packages/templates/tutorials/endless-runner-sample-game-87901) from the Unity Asset Store.
- **Unity Editor Version:** You can use any version of the Unity Editor for this project.

## Deliverables

- Code uploaded to a Git repository.
- Video walkthrough recorded using a screen recording tool explaining the code and demonstrating the tests.

## How to Run the Tests

1. Clone the repository to your local machine.
2. Open the project in Unity.
3. Run the Playmode tests through Unity’s Test Runner in the Editor. 
4. Ensure that the game runs as expected and all tests pass.
