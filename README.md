# Link
https://the-painkiller.github.io/fsm-character-controller-basic/

# Introduction

A simple character controller using Finite State Machines design pattern, for practice.

The character as well as the animations were all taken from Mixamo...and may not look quite good, but my purpose was to just practice FSM, so meh!

# Controls
- WSAD:               Move
- Space:              Jump
- Ctrl:               Crouch
- LMB:                Strike
- LMB Thrice Quickly: Special Strike
- RMB:                        Defend

# Mechanisms
A basic controller with movement, jump, crouch, attack and defence mechanisms. 

Fighting mode changes how the player moves a little bit. If you play some big games like Witcher or Assassin's Creed, you will see that in normal mode, if you press A or Left, the character turns 90 degrees in that direction and moves along that axis. But when fighting, this changes to the character strafing rather than turning around. This is to sort of "keep an eye" on the enemy target. This is reflected in this sample too.

# Documentation

The state machine has been divded into two separate parts in this case. Movement states and Action states. Movement states take care of all the movement like walk, run, crouch, jump & idle; whereas Action states are for Attack, Defend or None.

## ActionStates & MovementStates
All action state classes are inherited by ActionState base class. All the movement state classes are inherited by MovementState base class. Within FSMManager, each state is referenced as these base classes only. Any animation change or animator inputs are passed to AnimationManager from these state classes as well.


## FSMManager
FSMManager is the main manager class that controls the finite states of both, actions and movement. This class is the one responsible for listening to inputs through InputControlsManager, collaborate with CharacterMovementManager, and control which state to be transitioned to. Each time a state is to be transitioned, the previous state is first Exit, new state Entered and then Executed. This is what FSMManager has to take care of.

The Execute() methods of the currently active states run within the Update() loop of FSMManager as well.

## CharacterMovementManager
CharacterMovementManager is the one that controls the tranform of the character object in general. Any position change, orientation change, jump, etc, are taken care of by CharacterMovementManager. Other classes calling this class have to pass just the final values, and to calculate and transition to them in a proper way, is the responsibility of this class.

## AnimationEventManager
A really simple class that is attached to the object on which Animator component is attached to, this class is used to trigger AnimationEvents from the character's animation clips. In current scenario they only trigger towards the end of an Attack clip, so that your character completes one attack before accepting another.

##AnimationManager
AnimationManager is responsible to deal with Animator directly. All the parameters are controlled here and any other class wanting to pass values to a parameter has to call this class to do so.

## CameraController
This class simply controls the camera. It accepts a second transform object as an X-Axis gimbal, and is responsible for the rotation/movement of the camera.

## InputControlsManager
This class is responsible to listen to all the actions triggered by the InputControls class and fire proper events in return. These events are then listened to by FSMManager or any other class, to perform specific input based actions.

## InputControls
InputControls is just a generated class acquired by creating an Input Action Asset for the inputs.

## PlayerControlSettings
This is a ScriptableObject class that takes care of various properties of the character, such as, speed, jump strength, etc.
