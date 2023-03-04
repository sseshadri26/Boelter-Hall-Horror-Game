# DEV LOG -- UI Team

## About
This is a simple log for tracking the current and past challenges taken on by the UI programming team over the course of the project. We're doing this to keep a record of *why* we're making the choices that we are so that it's easier to understand how the codebase came to be and how to improve it.

## Log

### (2/12)
**Problem:** Making scripts that interface with the controls on a UI Document is tedious!
- It's a tedious process of scanning through all the buttons or text fields (or whatever) and explicitly writing code to address each one.
- **Resolution**: Make an editor tool that auto-generates a script for a specified UI Document. While this will require a lot of effort up front, we believe it will pay back huge dividends later on, especially for some of the more complex UI in the game.

### (2/6)
**Problem:** What strategy do we employ to wire up the UI events to the main game?
- For buttons/input widgets that interact with the game, such as resume, restart, main menu, etc., we could send out a message through a `ScriptableObject` channel that the game listens to. 
- An alternative would be to use a `static` class as the interface between the UI and the game, with public events exposed. Global data/functions are always dangerous because they can impact who knows how many other pieces of code tied to them -- their scope is unknowable. The fact that a `static` event would be accessible from all parts of the codebase means it's super easy for developers to add code that either calls it or acts when it's called, meaning it gets super hard to debug since there are so many ways that event could have been triggered and so many different parts of the code that may have been modified because of it.
- Note that a `static` class could also serve as a channel like `ScriptableObject` can, with the main difference being that it's publicly accessible. The whole point of a channel is that it's a stable interface: all other modules that tie themselves to it (on either the receiving or sending end) won't have to worry about changing their code that interacts with the interface that frequently (since the interface is unlikely to change).

- **Resolution:** We will use `ScriptableObject` for event channels. The antidote to global data is dependency injection. Here, we are *injecting* our event channel object into the modules that use them instead of fetching it ourselves through a global class, making the decision to link up to an event channel more intentional and less prone to errors caused by carelessness. Not only this, but Aaron's made a Unity editor tool in the past that already tests `ScriptableObject` channels, so the overhead of creating a testing framework is mostly already covered.

**Problem:** What data should the Pause Menu pass to the game controller?
- Creating a separate event channel for each possible message (like `OnResumePressed`, `OnRestartPressed`, etc.) seems like overkill since the `GameManager` will need to specifically subscribe to each of these events, which can be tedious. The key point here is that each of these individual events would likely be handled by the same subscriber(s), so there's not much point to splitting them off.
- **Resolution:** An alternative would be to have a single event channel for game state manipulation, and enums representing different commands like `RESUME`, `RESTART`, or `GO_MAIN_MENU` could be passed through. Currently, we don't see any significant disadvantages of doing it this way than having a dedicated channel for each.

### (2/5)
**Problem:** UI Toolkit or Old UI System?
- UI Toolkit makes it easier to design precise UI, but comes with the drawback of having only rough documentation and more complex implementations
- **Resolution:** We are chosing to use UI Toolkit to better manage the complex structure of the game's UI, which currently requires tabbed panels, embedded diagrams (like for items and the map), in addition to "stack" panels that require scrolling to view different items. Additionally, since UI Toolkit is the direction Unity is currently moving in, we reason that using it will help the project's UI stay supported far into the future.

