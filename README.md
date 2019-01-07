# RTS Multiplayer Prototype

This is a learning exercise to study how to build an RTS-style game using the Unity engine.  This uses the Unity *Unet* networking system, which does not utilize a lockstep networking model like most robust RTS engines. The *Unet* API has been deprecated in Unity v2018.3 however the replacement API currently only offers a FPS-focused model and so does not offer any significant benefits over the *Unet* model.

## Features 
* RTS style mouse controls :
  * Drag-select
  * Click-select
  * Right-click to move/attack
  * Attack + move
* Server-authoritative control, even for the "host" player
* Networked object pooling

## ToDo
* Make units destructable
* Make the game winnable by destroying all enemy units
* Add terrain more complex than a flat plane
* Add navigation that handles multiple units moving in a group
* Add parabolic and homing weapons
* Add per-unit special abilities
* 
