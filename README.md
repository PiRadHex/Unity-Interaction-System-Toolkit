# Unity Interaction System Toolkit

The **Interaction System Toolkit** is a collection of versatile scripts for creating interactive objects within your Unity projects. Whether you want to create objects that can be picked up, trigger events, change materials, or even switch cameras, this toolkit provides a set of ready-to-use components to streamline your game development process. This toolkit is open-source and free to use.

<p align="center"><
![SharedScreenshot29_1](https://github.com/PiRadHex/Unity-Interaction-System-Toolkit/assets/124064917/820c6825-689e-4131-8177-3647bebcb41b)
/></p>

## Table of Contents

1. [Introduction](#introduction)
2. [WebGL Demo](#webgl-demo)
3. [Key Features](#key-features)
4. [Usage](#usage)
7. [Contributing](#contributing)
8. [License](#license)

## Introduction

Creating interactive objects in Unity can be a time-consuming task, but with the **Interaction System Toolkit**, you can easily add interactivity to your game objects. The toolkit includes a variety of scripts that can be applied to objects in your game to make them interactive with minimal effort.

## WebGL Demo

Check out the WebGL demo scene to see the toolkit in action: [Interaction System Toolkit Demo](https://piradhex.itch.io/interaction-system-toolkit)

## Key Features

+ **Interactable**: The base class for all interactable objects in the toolkit. It includes settings for interaction radius, interaction transform, and more. This class handles the core interaction logic.

+ **ChangeTransform**: A script that allows you to change an object's position, rotation, and scale based on player interaction. It provides smooth transitions between different states, making it suitable for creating doors, elevators, or any objects with dynamic transformations.

+ **MultipleInteract**: This script enables you to create objects that trigger multiple interactions with other objects. It can be used to trigger multiple actions simultaneously, such as opening multiple doors or activating various mechanisms at once.

+ **OnTriggerInteract**: A component that lets you trigger interactions when a GameObject enters or exits a trigger collider. It offers fine-grained control over which objects can trigger interactions and can be useful for creating switches, buttons, or interactive zones.

+ **PlayerSwitch**: A script that allows you to switch between different virtual cameras and player control states. It's useful for changing the player's perspective or controlling different characters in your game.

+ **ChangeVCamPriority**: This script enables you to change the priority of a Cinemachine Virtual Camera when interacted with. It can be used for creating camera switching mechanisms to provide different perspectives or cinematic effects.

+ **Counter**: Keeps track of a numerical value and allows for incrementing or decrementing it.

+ **PlayAudio**: Plays or pauses audio sources and supports various audio settings.

+ **LoadScene**: Loads a new scene when interacted with.

+ **MaterialSwitch**: Switches between two materials on a target renderer.

+ **ImageColorSwitch**: Switches the color of a Unity UI Image component.

## Usage

To use the **Interaction System Toolkit** in your Unity project, follow these steps:

1. Clone or download this repository.
2. Add the toolkit to your Unity project by importing the provided package or copying the relevant scripts to your project.
3. Attach the appropriate scripts to your game objects to make them interactive.

Each script has its own specific use case and can be added to Unity GameObjects to create various interactive elements in your game. You can refer to the individual script descriptions for more details on how to use them.

+ Note: Make sure you have the necessary components and dependencies in your project, such as Cinemachine, and TextMeshPro.

### Demo Scene

To see the Interaction System Toolkit in action, we have provided a demo scene that showcases the possible usage scenarios for these scripts. You can test the functionality of the toolkit in this scene.

## Contributing

We welcome contributions to the **Interaction System Toolkit**. If you have ideas for new features, improvements, or bug fixes, please feel free to contribute to the repository by creating pull requests or opening issues. We appreciate your help in making this toolkit even more versatile and user-friendly.

## License

The **Interaction System Toolkit** is open-source software licensed under the MIT License. You are free to use, modify, and distribute the toolkit in your projects.

---

Feel free to explore the toolkit and start making your Unity game more interactive and engaging.

Happy game development! 🎮
