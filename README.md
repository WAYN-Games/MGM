# Disclaimer
This is a personal learning project. It's a work in progress using lots of unity preview parckages. It's kept up to date with the latest releases of each package (meaning stuff can break any time). Therefore, it is provided as is with no garantees.

# MGM
Stands for Modular Game Mechanics.

* MGM/Core : Contains Components and mechanincs shared by other modules like physics cast util or base input classes

* MGM/Movement : Contains basic mouvement mechanics
 * Physics based move
 * Physics based jump
 * Aim rotation

* MGM/TankDemo : Contain a small demo scene based on the Unity tank demo assets
  * Build :
    * Standalone : use MGM/TankDemo/Settings/StandaloneBuildSettings to build for PC.
  * Inputs :
    * Keyboard/Mouse : WASD to move, mouse to aim
    * GamePad : Left Stick to move, Right Stick to aim
  * Known issues : 
    * subscene must be closed before entering playmode otherwise, the level art is messed up.
