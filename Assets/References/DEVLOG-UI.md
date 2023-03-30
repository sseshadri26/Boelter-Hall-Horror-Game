# DEV LOG -- UI Team

## About
This is a simple log for tracking the current and past challenges taken on by the UI programming team over the course of the project. We're doing this to keep a record of *why* we're making the choices that we are so that it's easier to understand how the codebase came to be and how to improve it.

## Log

### (3/21)
**Problem:** The inventory UI panel has a special feature where key items are marked in a special color. How do we allow this unique functionality to be injected into the main `AlamancUI.cs` code?
- It's important to keep in mind the pattern of separating visuals from behavior. A good solution would be one that shifted the changing of the item colors to the UI itself. Sounds like USS classes could be used here?
- Define an AlamanacUI USS class called `marked` or something in order to apply the special formatting to the given item. While this does inherently couple the marking feature to being an AlmanacUI even when not all alamanacs will use it, I think it's small enough of a feature that the coupling won't be too significant even in the worst case where no other almanac uses it.
- I tried using inheritance with derived classes handling styling of list item UI. It worked well except for the fact that derived classes are stuck dealing with abstract `ItemSO` objects that hide important details the derived classes need to use (for example, InventoryUI needs to know specific properties that `InventoryItemSO` has but not `ItemSO`). I could have just casted `ItemSO` references to `InventoryItemSO`, but then it would require a runtime check that may cause some confusing errors. 

- **Resolution:** Use a component-based approach where a separate component is responsible for generating the UI for the individual item elements. This way, there is a compile time check on the type of the item collection/items (for example, we are guaranteed it is `InventoryItemSO` and not `JournalItemSO` at compile time -- in fact the editor won't even let you use it). This way, we catch bugs earlier at the cost of a little bit of complexity with adding a brand new component.

### (3/16)
**Problem:** Both Inventory and Journal UI panels essentially have the same functionality but just different visual representations (a list of items that let you view each in more detail). How do we reduce code repeat?
- Need a strategy for separating visuals from functionality.
- The whole point of `UIElements` is to cleanly separate UI design from UI code. How can we leverage this separation? Well it seems like the paradigm is to simply agree to use the same class names. But what about slight differences between Inventory and Journal like the ability to highlight names in the Inventory? As long as there aren't too many of these differences, we can just have special class names that only appear in some UI designs but not others.
- **Resolution:** Standardize the class names used in almanac-style UI. For example, there should be a `main-title` class for representing the text component for the currently selected item. We can use a single class to provide the functionality for both the Inventory and Journal panels called `AlamanacUI.cs` or something like that. This class would also leave the individual item card UIs to be injected just like `InventoryUI.cs` currently does. Note that we don't want to just be injecting everything since that would make the code more complex, far too complex for the scale of code reuse we need (probably only used for Inventory, Journal, and maybe one or two more in the future).

### (3/9)
**Problem:** All panels will need functionality to enter/exit the screen. How do we implement this efficiently?
- This is a design choice between inheritance and components. Components are great for building highly-variable behaviors that may use widely different combinations of behaviors. However, you pay for this flexibility with complexity -- you need a system for communication between components and a system for storing them (Unity already does this for you with `GameObject`). Inheritance is simpler to implement but does come with the downside of forcing all derived classes to have certain functionality.

- **Resolution:** Write a base class for all UI panels in the game that has built-in animation functionality. All derived classes need not worry about how to animate the panel in or out. Even though inheritance tightly couples animating to panels, this is fine because pretty much all panels will need some way to animate in. Inheritance is simpler to implement and understand than other options, so it seemed like a reasonable choice.

### (2/12)
**Problem:** Making scripts that interface with the controls on a UI Document is tedious!
- It's a tedious process of scanning through all the buttons or text fields (or whatever) and explicitly writing code to address each one.
- **Resolution:** Make an editor tool that auto-generates a script for a specified UI Document. While this will require a lot of effort up front, we believe it will pay back huge dividends later on, especially for some of the more complex UI in the game.

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

