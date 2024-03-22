# EAnimationChecker

`EAnimationChecker` is a Unity Editor tool designed for checking Animator Controllers, Animator Override Controllers, and Animators in scene objects. It identifies any Animator States or Blend Trees missing animations, ensuring all necessary animations are assigned and helping maintain animation integrity across your Unity project.
![EAnimationChecker Window](/EAnimationChecker.png)

## Features
1. Check Animator Controllers: Scans Animator Controllers in the "Assets" directory for states without assigned animations.
2. Check Animator Override Controllers: Identifies Animator Override Controllers in the "Assets" directory with unassigned or original clips.
3. Check Scene Animators: Finds Animator components in the current scene missing Animator Controllers.

## Installation

1. Copy `EAnimationChecker.cs` into your Unity project.

## How to Use

1. Open the tool from the Unity Editor Menu under "Escripts > EAnimationChecker".
2. Select the desired check to perform from the sub-menu options.

This tool streamlines the process of ensuring animations are properly assigned, helping to avoid runtime errors and improve the animation workflow in Unity projects.
