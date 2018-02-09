# VRLM - Virtual Reality Locomotion

## Special Thanks!

* Anthony Bukowski
* Masatoshi Chang-Ogimoto
* Lee Wasilenko
* Harvey Ball
* Manchester Metropolitan University

## What is this?

VRLM is a package of unusual virtual reality locomotion methodologies which aims to counter virtual reality sickness. The package currently features 3 locomotion methodologies which were created for a final year ungraduate research project. 
The research aimed to find ways to counter virtual reality using different way of moving within a 3D virtual reality world. The research has been approved funding for further research by the Manchester Metropolitan University. Results will be written up as a research paper and published (We're aiming for IEEE).


## How do I use it?

It's as simple as drag and drop the scripts onto the `[CameraRig]`.

1. Drag and drop the `VRPlayerPresets` into the `[CameraRig]`.
2. Drag and drop the `ControllerEvents` onto both controllers.
3. Drag and drop your selected movement script into the `[CameraRig]`' and assign all the relevant values required. Many can be left untouched, but it is mandatory to manually assign the `[CameraRig]`'s left and right controllers to the relative `LcontrollerEvents` and `RcontrollerEvents` or It will throw an error back at you.

## Can I help or provide feedback?

Certiantly you can, Just drop a message on the [issues section](https://github.com/Clavilux/VRLM/issues "VRLM Issues page") of this reposetory, I'm always looking for feedback.

## Change Log

### 13/11/2017 V1.20
* **Edit (README)** Fixed the mistake on the 'How do I use it' section.

### 27/07/2017 v1.10 
* All locomotion scripts are updated with relevant tool tips and titles. 
* **Fix (JogOnSpotMovement)** Players would automatically move when on a slope due to the collider constantly pushing back the player from the slope, resulting in a difference in height that surpasses the threshold and adding movement to the `[CameraRig]`. We fixed this by tracking the Y distance between an empty `GameObject` (`zeroTracker`) placed at 0, 0, 0 locally inside the `[CameraRig]` instead of the headset’s Y movement in game. Developers can either assign a `GameObject` of their own or leave the field empty for the script to automatically assign one.
* **Edit (LeanMovement)**  `CentreTracker` `GameObject` has been renamed to `zeroTracker` to fit a uniform.
* **Edit (JogOnSpotMovement & LeanMovement)** `setTrackerObj` made to take in a `GameObject` argument. 
* **Edit (LeanMovement & ArmMovement)** Added `setMovementInformation` and merged `calculateMovementSpeed` into new method.

