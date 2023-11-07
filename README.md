# Unity Interaction System Toolkit

This Unity Interaction System Toolkit is designed to provide a set of versatile scripts for creating interactive objects within your Unity projects. It allows you to easily implement interactive objects that the player can interact with, such as enemies, items, switches, and more.

## Key Features

+ Interactable: A base class for creating interactive objects with adjustable interaction radius and optional auto-open/close functionality. This class handles the core interaction logic.

+ ChangeTransform: A script that allows you to change an object's position, rotation, and scale based on player interaction. It provides smooth transitions between different states, making it suitable for creating doors, elevators, or any objects with dynamic transformations.

+ MultipleInteract: This script enables you to create objects that trigger multiple interactions with other objects. It can be used to trigger multiple actions simultaneously, such as opening multiple doors or activating various mechanisms at once.

+ OnTriggerInteract: A component that lets you trigger interactions when a GameObject enters or exits a trigger collider. It offers fine-grained control over which objects can trigger interactions and can be useful for creating switches, buttons, or interactive zones.

+ PlayerSwitch: A script that allows you to switch between different virtual cameras and player control states. It's useful for changing the player's perspective or controlling different characters in your game.

+ ChangeVCamPriority: This script enables you to change the priority of a Cinemachine Virtual Camera when interacted with. It can be used for creating camera switching mechanisms to provide different perspectives or cinematic effects.

## How to Use

Each script has its own specific use case and can be added to Unity GameObjects to create various interactive elements in your game. You can refer to the individual script descriptions for more details on how to use them.
+ Note: Make sure you have the necessary components and dependencies in your project, such as Cinemachine, if you plan to use the ChangeVCamPriority script.

## Demo Scene

To see the Interaction System Toolkit in action, we have provided a demo scene that showcases the possible usage scenarios for these scripts. You can test the functionality of the toolkit in this scene.
