# Touch support

* [`Touchscreen` Device](#touchscreen-device)
* [`Touch` class](#enhancedtouchtouch-class)
    * [Using Touch with Actions](#using-touch-with-actionsactionsmd)

Touch support is divided into:
* low-level support implemented in the [`Touchscreen`](#touchscreen-device) class.
* high-level support implemented in the [`EnhancedTouch.Touch`](#enhancedtouchtouch-class) class.

Touch input is supported on Android, iOS, Windows, and UWP.

## `Touchscreen` Device

At the lowest level, a touch screen is represented by a [`Touchscreen`](../api/UnityEngine.InputSystem.Touchscreen.html) Device which captures the touch screen's raw state. Touch screens are based on the [`Pointer`](Pointers.md) layout.

You can query the touch screen that was last used or last added using [`Touchscreen.current`](../api/UnityEngine.InputSystem.Touchscreen.html#UnityEngine_InputSystem_Touchscreen_current).

### Controls

Additional to the [Controls inherited from `Pointer`](Pointers.md#controls), touch screen Devices implement the following Controls:

|Control|Type|Description|
|-------|----|-----------|
|[`primaryTouch`](../api/UnityEngine.InputSystem.Touchscreen.html#UnityEngine_InputSystem_Touchscreen_primaryTouch)|[`TouchControl`](../api/UnityEngine.InputSystem.Controls.TouchControl.html)|A touch Control representing the primary touch of the screen. The primary touch is the touch driving the [`Pointer`](Pointers.md) representation of the Device.|
|[`touches`](../api/UnityEngine.InputSystem.Touchscreen.html#UnityEngine_InputSystem_Touchscreen_touches)|[`ReadOnlyArray<TouchControl>`](../api/UnityEngine.InputSystem.Controls.TouchControl.html)|An array of touch Controls, representing all the touches on the Device.|

A touch screen Device consists of multiple [`TouchControls`](../api/UnityEngine.InputSystem.Controls.TouchControl.html). Each of these represents a potential finger touching the Device. The [`primaryTouch`](../api/UnityEngine.InputSystem.Touchscreen.html#UnityEngine_InputSystem_Touchscreen_primaryTouch) Control represents the touch which is currently driving the [`Pointer`](Pointers.md) representation, and which should be used to interact with the UI. This is usually the first finger to have touched the screen.
 [`primaryTouch`](../api/UnityEngine.InputSystem.Touchscreen.html#UnityEngine_InputSystem_Touchscreen_primaryTouch) is always identical to one of the entries in the [`touches`](../api/UnityEngine.InputSystem.Touchscreen.html#UnityEngine_InputSystem_Touchscreen_touches) array. The [`touches`](../api/UnityEngine.InputSystem.Touchscreen.html#UnityEngine_InputSystem_Touchscreen_touches) array contains all touches the system can track. This array has a fixed size, regardless of how many fingers are currently active. If you need an API that only represents active touches, look at the higher-level [`EnhancedTouch.Touch` class](#enhancedtouchtouch-class).

Each [`TouchControl`](../api/UnityEngine.InputSystem.Controls.TouchControl.html) on the Device, including [`primaryTouch`](../api/UnityEngine.InputSystem.Touchscreen.html#UnityEngine_InputSystem_Touchscreen_primaryTouch), is made up of the following child Controls.

|Control|Type|Description|
|-------|----|-----------|
|[`position`](../api/UnityEngine.InputSystem.Controls.TouchControl.html#UnityEngine_InputSystem_Controls_TouchControl_position)|[`Vector2Control`](../api/UnityEngine.InputSystem.Controls.Vector2Control.html)|Absolute position on the touch surface.|
|[`delta`](../api/UnityEngine.InputSystem.Controls.TouchControl.html#UnityEngine_InputSystem_Controls_TouchControl_delta)|[`Vector2Control`](../api/UnityEngine.InputSystem.Controls.Vector2Control.html)|The difference in `position` since the last frame.|
|[`startPosition`](../api/UnityEngine.InputSystem.Controls.TouchControl.html#UnityEngine_InputSystem_Controls_TouchControl_startPosition)|[`Vector2Control`](../api/UnityEngine.InputSystem.Controls.Vector2Control.html)|The `position` where the finger first touched the surface.|
|[`startTime`](../api/UnityEngine.InputSystem.Controls.TouchControl.html#UnityEngine_InputSystem_Controls_TouchControl_startTime)|[`DoubleControl`](../api/UnityEngine.InputSystem.Controls.IntegerControl.html)|The time when the finger first touched the surface.|
|[`press`](../api/UnityEngine.InputSystem.Controls.TouchControl.html#UnityEngine_InputSystem_Controls_TouchControl_press)|[`ButtonControl`](../api/UnityEngine.InputSystem.Controls.ButtonControl.html)|Whether the finger is pressed down.|
|[`pressure`](../api/UnityEngine.InputSystem.Controls.TouchControl.html#UnityEngine_InputSystem_Controls_TouchControl_pressure)|[`AxisControl`](../api/UnityEngine.InputSystem.Controls.AxisControl.html)|Normalized pressure with which the finger is currently pressed while in contact with the pointer surface.|
|[`radius`](../api/UnityEngine.InputSystem.Controls.TouchControl.html#UnityEngine_InputSystem_Controls_TouchControl_radius)|[`Vector2Control`](../api/UnityEngine.InputSystem.Controls.Vector2Control.html)|The size of the area where the finger touches the surface.|
|[`touchId`](../api/UnityEngine.InputSystem.Controls.TouchControl.html#UnityEngine_InputSystem_Controls_TouchControl_touchId)|[`IntegerControl`](../api/UnityEngine.InputSystem.Controls.IntegerControl.html)|The ID of the touch, used to distinguish individual touches.|
|[`phase`](../api/UnityEngine.InputSystem.Controls.TouchControl.html#UnityEngine_InputSystem_Controls_TouchControl_phase)|[`TouchPhaseControl`](../api/UnityEngine.InputSystem.Controls.TouchPhaseControl.html)|A Control that reports the current  [`TouchPhase`](../api/UnityEngine.InputSystem.TouchPhase.html) of the touch.|
|[`tap`](../api/UnityEngine.InputSystem.Controls.TouchControl.html#UnityEngine_InputSystem_Controls_TouchControl_tap)|[`ButtonControl`](../api/UnityEngine.InputSystem.Controls.ButtonControl.html)|A button Control which reports whether the OS recognizes a tap gesture from this touch.|
|[`tapCount`](../api/UnityEngine.InputSystem.Controls.TouchControl.html#UnityEngine_InputSystem_Controls_TouchControl_tapCount)|[`IntegerControl`](../api/UnityEngine.InputSystem.Controls.ButtonControl.html)|If [`tap`](../api/UnityEngine.InputSystem.Controls.TouchControl.html#UnityEngine_InputSystem_Controls_TouchControl_tap) is reported as pressed, `tapCount` reports the number of consecutive taps as recognized by the OS. You can use this to detect double- and multi-tap gestures.|

### Using touch with [Actions](Actions.md)

Touch input can be used with Actions like any other [`Pointer`](Pointers.md) Device, by [binding](ActionBindings.md) to the [pointer Controls](Pointers.md#controls), like `<Pointer>/press` or `<Pointer>/delta`. This will get you input from the primary touch (as well as from any other non-touch pointer Devices).

However, if you care about getting input from multiple touches in your Action, you can bind to individual touches by using Bindings like `<Touchscreen>/touch3/press`, or use a wildcard Bindings to bind one Action to all touches like this: `<Touchscreen>/touch*/press`. If you bind a single Action to input from multiple touches, you should set the Action type to [pass-through](Actions.md#pass-through) so the Action gets callbacks for each touch, instead of just one.

## `EnhancedTouch.Touch` Class

The [`EnhancedTouch.Touch`](../api/UnityEngine.InputSystem.EnhancedTouch.Touch.html) class provides enhanced touch support. To enable it, call [`EnhancedTouchSupport.Enable()`](../api/UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.html#UnityEngine_InputSystem_EnhancedTouch_EnhancedTouchSupport_Enable):

```
    using UnityEngine.InputSystem.EnhancedTouch;
    // ...
    // Can be called from MonoBehaviour.Awake(), for example. Also from any
    // RuntimeInitializeOnLoadMethod code.
    EnhancedTouchSupport.Enable();
```

>__Note__: [`Touchscreen`](../api/UnityEngine.InputSystem.Touchscreen.html) doesn't require [`EnhancedTouchSupport`](../api/UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.html) to be enabled. Touch works with [Actions](Actions.md) without using [`EnhancedTouchSupport`](../api/UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.html). Calling [`EnhancedTouchSupport.Enable()`](../api/UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.html#UnityEngine_InputSystem_EnhancedTouch_EnhancedTouchSupport_Enable) is only required if you want to use the [`EnhancedTouch.Touch`](../api/UnityEngine.InputSystem.EnhancedTouch.Touch.html) API.

The touch API is designed to provide access to touch information along two dimensions:

1. By finger: Each finger is defined as the Nth contact source on a [`Touchscreen`](../api/UnityEngine.InputSystem.Touchscreen.html). You can use  [Touch.activeFingers](../api/UnityEngine.InputSystem.EnhancedTouch.Touch.html#UnityEngine_InputSystem_EnhancedTouch_Touch_activeFingers) to get an array of all currently active fingers.

2. By touch: Each touch is a single finger contact with at least a beginning point ([`PointerPhase.Began`](../api/UnityEngine.InputSystem.TouchPhase.html)) and an endpoint ([`PointerPhase.Ended`](../api/UnityEngine.InputSystem.TouchPhase.html) or [`PointerPhase.Cancelled`](../api/UnityEngine.InputSystem.TouchPhase.html)). In-between those two points, an arbitrary number of [`PointerPhase.Moved`](../api/UnityEngine.InputSystem.TouchPhase.html) and/or [`PointerPhase.Stationary`](../api/UnityEngine.InputSystem.TouchPhase.html) records exist. All records in a touch will have the same [`touchId`](../api/UnityEngine.InputSystem.Controls.TouchControl.html#UnityEngine_InputSystem_Controls_TouchControl_touchId). You can use  [Touch.activeTouches](../api/UnityEngine.InputSystem.EnhancedTouch.Touch.html#UnityEngine_InputSystem_EnhancedTouch_Touch_activeTouches) to get an array of all currently active touches. This lets you track how a specific touch has moved over the screen, which is useful if you want to implement recognition of specific gestures.

See the [Scripting API Reference for the `EnhancedTouch.Touch`](../api/UnityEngine.InputSystem.EnhancedTouch.Touch.html) API for more info.

>__Note__: The [`Touch`](../api/UnityEngine.InputSystem.EnhancedTouch.Touch.html) and [`Finger`](../api/UnityEngine.InputSystem.EnhancedTouch.Finger.html) APIs don't generate GC garbage. The bulk of the data is stored in unmanaged memory that is indexed by wrapper structs. All arrays are pre-allocated.
