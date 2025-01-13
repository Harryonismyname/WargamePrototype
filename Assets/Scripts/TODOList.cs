
// TODO: Make a Selection system with two phases,
//       One that Governs friendly agent selection,
//       One that Governs hostile agent selection.
//       The Visual can be communicated via allies appearing on the left,
//       and hostiles appearing on the right.
//       actions need to have a confirm step to prevent misclicks
// TODO: Add action states
//       Actions have 3 states:
//           - Selection
//           - Declaration
//           - Confirmation
//       Selection:
//           When an allied agent is selected, the player can select from a list of available actions
//           These actions when clicked on will change the state to the Declaration state
//       Declaration:
//           During this state the player can select a location, other agents, or whatever is neccesary
//           to complete the action.
//       Confirmation:
//           This is where the action is actually performed.
// TODO: Backout of looking at the systems so intricately and just build out the game flow system
// TODO: Create a testing script to help troubleshoot the game flow
// TODO: Block out interaction systems into their own system independent of the gameflow
// TODO: Design these systems before building them
