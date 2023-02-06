# DEV LOG -- UI Team

## About
This is a simple log for tracking the current and past challenges taken on by the UI programming team over the course of the project. We're doing this to keep a record of *why* we're making the choices that we are so that it's easier to understand how the codebase came to be and how to improve it.

## Log
### (2/5)
**Problem:** UI Toolkit or Old UI System?
- UI Toolkit makes it easier to design precise UI, but comes with the drawback of having only rough documentation and more complex implementations
- **Resolution:** We are chosing to use UI Toolkit to better manage the complex structure of the game's UI, which currently requires tabbed panels, embedded diagrams (like for items and the map), in addition to "stack" panels that require scrolling to view different items. Additionally, since UI Toolkit is the direction Unity is currently moving in, we reason that using it will help the project's UI stay supported far into the future.

