# Disclaimer
This is a personal learning project. It's a work in progress using lots of unity preview parckages. It's kept up to date with the latest releases of each package (meaning stuff can break any time). Therefore, it is provided as is with no garantees.

# MGM
Stands for Modular Game Mechanics.

* MGM/Core : Contains Components and mechanincs shared by other modules like physics cast util or base input classes
  * Registry Based Evnet system : the aim is to be able to schedule prefiveied events from just a reference. This allow to make a modular effect system and trigger the effect knowing only there reference, not there content.

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
    
# Known Issue
* the registry based event system does not support empty event types
* the event don't get into the registry when declared in a closed subscene. This will probably require to move the registry to another implementation (maybe scriptable object.
