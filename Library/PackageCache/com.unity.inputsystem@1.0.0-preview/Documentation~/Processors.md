# Processors

An Input Processor takes a value and returns a processed result for it. The received value and result value must be of the same type. For example, you can use a [clamp](#clamp) Processor to clamp values from a control to a certain range.

>__Note__: To convert received input values into different types, see [composite Bindings](ActionBindings.md#composite-bindings).

* [Using Processors](#using-processors)
    * [Processors on Bindings](#processors-on-bindings)
    * [Processors on Actions](#processors-on-actions)
    * [Processors on Controls](#processors-on-controls)
* [Predefined Processors](#predefined-processors)
    * [Axis deadzone](#axis-deadzone)
    * [Clamp](#clamp)
    * [Invert](#invert)
    * [Invert Vector 2](#invert-vector-2)
    * [Invert Vector 3](#invert-vector-3)
    * [Normalize](#normalize)
    * [Normalize Vector 2](#normalize-vector-2)
    * [Normalize Vector 3](#normalize-vector-3)
    * [Scale](#scale)
    * [Scale Vector 2](#scale-vector-2)
    * [Scale Vector 3](#scale-vector-3)
    * [Stick deadzone](#stick-deadzone)
* [Writing custom Processors](#writing-custom-processors)

## Using Processors

Processors can be installed on [bindings](ActionBindings.md), [actions](Actions.md) or on [controls](Controls.md).

### Processors on Bindings

When you create Bindings for your [actions](Actions.md), you can choose to add Processors to the Bindings to process the values read from the controls they bind to, before applying them to the action value. For instance might want the `Vector2` values coming from the controls to be inverted along the Y axis before passing these values to your action that drives the input logic for your app. You can do this by adding an [Invert Vector2](#invert-vector-2) Processor to your Binding.

If you are using [Input Action Assets](ActionAssets.md), you can add any Processor to your Bindings in the input action editor. Once you [created some Bindings](ActionAssets.md#editing-bindings), select the Binding you want to add Processors to so that the right pane of the window shows the properties for that Binding. Click on the plus icon on the __Processors__ foldout to open a list of all available Processors matching your control type. Then, choose a Processor type to add a Processor instance of that type. The Processor now shows under the __Processors__ foldout. If the Processor has any parameters you can now edit them here as well:

![Binding Processors](Images/BindingProcessors.png)

To remove a Processor, click the minus button next to it. You can also click the up and down arrows to change the order of Processors, which affects in which order values are being processed.

If you create your Bindings in code, you can add Processors like this:

```CSharp
var action = new InputAction();
action.AddBinding("<Gamepad>/leftStick")
    .WithProcessor("invertVector2(invertX=false)");
```

### Processors on Actions

Processors on Actions work in the same way as Processors on Bindings, but they affect all controls bound to an Action, not just the ones coming from a specific Binding. If there are Processors on both the Binding and the Action, the system processes the ones from the Binding first.

You can add and edit Processors on Actions in [Input Action Assets](ActionAssets.md) the [same way](#processors-on-bindings) as you would for Bindings: select an Action to edit, then add one or more Processors in the right window pane.

If you create your Actions in code, you can add Processors like this:

```CSharp
var action = new InputAction(processors: "invertVector2(invertX=false)");
```

### Processors on Controls

You can have any number Processors directly on an [`InputControl`](../api/UnityEngine.InputSystem.InputControl.html), which then process the values read from the Control. Whenever you call [`ReadValue`](../api/UnityEngine.InputSystem.InputControl-1.html#UnityEngine_InputSystem_InputControl_1_ReadValue) on a Control, all Processors on that Control will process the value before it gets returned to you. You can use [`ReadUnprocessedValue`](../api/UnityEngine.InputSystem.InputControl-1.html#UnityEngine_InputSystem_InputControl_1_ReadUnprocessedValue) on a Control to bypass the Processors.

Processors get added to a Control during device creation, if they are specified in the Control's [layout](Layouts.md). You can't add Processors to existing Controls after they've been created, so you can only add Processors to Controls when you're [creating custom devices](Devices.md#creating-custom-devices). The devices that the Input System supports out of the box already have some useful Processors added on their Controls. For instance, sticks on gamepads have a [Stick Deadzone](#stick-deadzone) Processor.

If you are using a layout generated by the Input System from a [state struct](Devices.md#step-1-the-state-struct) using [`InputControlAttributes`](../api/UnityEngine.InputSystem.Layouts.InputControlAttribute.html), you can specify the Processors to use through the [`processors`](../api/UnityEngine.InputSystem.Layouts.InputControlAttribute.html#UnityEngine_InputSystem_Layouts_InputControlAttribute_processors) property of the attribute, like this:

```CSharp
public struct MyDeviceState : IInputStateTypeInfo
{
    public FourCC format => return new FourCC('M', 'Y', 'D', 'V');

    // Add an axis deadzone to the Control to ignore values
    // smaller then 0.2, as our Control does not have a stable
    // resting position.
    [InputControl(layout = "Axis", processors = "AxisDeadzone(min=0.2)")]
    public short axis;
}
```

If you [create a layout from JSON](Layouts.md#layout-from-json), you can specify Processors on your Controls like this:

```
{
    "name" : "MyDevice",
    "extend" : "Gamepad", // Or some other thing
    "controls" : [
        {
            "name" : "axis",
            "layout" : "Axis",
            "offset" : 4,
            "format" : "FLT",
            "processors" : "AxisDeadzone(min=0.2)"
        }
    ]
}
```

## Predefined Processors

The Input System package comes with a set of useful Processors you can use.

### Axis Deadzone

|__Name__|`AxisDeadzone`|
|---|---|
|__Operand Type__|`float`|
|__Parameters__|`float min`<br>`float max`|

An axis deadzone Processor scales the values of a Control so that any value with an absolute value smaller than `min` is 0, and any value with an absolute value larger than `max` is 1 or -1. Many Controls don't have a precise resting point (that is, they don't always report exactly 0 when the Control is in the center). Using the `min` value on a deadzone Processor avoids unintentional input from such Controls. Also, some Controls don't consistently report their maximum values when moving the axis all the way. Using the `max` value on a deadzone Processor ensures that you always get the maximum value in such cases.

### Clamp

|__Name__|`Clamp`|
|---|---|
|__Operand Type__|`float`|
|__Parameters__|`float min`<br>`float max`|

Clamps input values to the [`min`..`max`] range.

### Invert

|__Name__|`Invert`|
|---|---|
|__Operand Type__|`float`|

Inverts (that is, multiplies by -1) the values from a Control.

### Invert Vector 2

|__Name__|`InvertVector2`|
|---|---|
|__Operand Type__|`Vector2`|
|__Parameters__|`bool invertX`<br>`bool invertY`|

Inverts (that is, multiplies by -1) the values from a Control. Inverts the x axis of the vector if `invertX` is true, and the y axis if `invertY` is true.

### Invert Vector 3

|__Name__|`InvertVector3`|
|---|---|
|__Operand Type__|`Vector3`|
|__Parameters__|`bool invertX`<br>`bool invertY`<br>`bool invertZ`|

Inverts (that is, multiplies by -1) the values from a Control. Inverts the x axis of the vector if `invertX` is true, the y axis if `invertY` is true, and the z axis if `invertZ` is true.

### Normalize

|__Name__|`Normalize`|
|---|---|
|__Operand Type__|`float`|
|__Parameters__|`float min`<br>`float max`<br>`float zero`|

Normalizes input values in the range [`min`..`max`] to unsigned normalized form [0..1] if `min` is >= `zero`, and to signed normalized form [-1..1] if `min` < `zero`.

### Normalize Vector 2

|__Name__|`NormalizeVector2`|
|---|---|
|__Operand Type__|`Vector2`|

Normalizes input vectors to be of unit length (1). This is the same as calling `Vector2.normalized`.

### Normalize Vector 3

|__Name__|`NormalizeVector3`|
|---|---|
|__Operand Type__|`Vector3`|

Normalizes input vectors to be of unit length (1). This is the same as calling `Vector3.normalized`.

### Scale

|__Name__|`Scale`|
|---|---|
|__Operand Type__|`float`|
|__Parameters__|`float factor`|

Multiplies all input values by `factor`.

### Scale Vector 2

|__Name__|`ScaleVector2`|
|---|---|
|__Operand Type__|`Vector2`|
|__Parameters__|`float x`<br>`float y`|

Multiplies all input values by `x` along the X axis and by `y` along the Y axis.

### Scale Vector 3

|__Name__|`ScaleVector3`|
|---|---|
|__Operand Type__|`Vector3`|
|__Parameters__|`float x`<br>`float y`<br>`float x`|

Multiplies all input values by `x` along the X axis, by `y` along the Y axis and by `z` along the Z axis.

### Stick deadzone

|__Name__|`StickDeadzone`|
|---|---|
|__Operand Type__|`Vector2`|
|__Parameters__|`float min`<br>`float max`|

A stick deadzone Processor scales the values of a Vector2 Control, such as a stick, so that input vector with a magnitude smaller than `min` results in (0,0), and any input vector with a magnitude greater than `max` is normalized to length 1. Many Controls don't have a precise resting point (that is, they don't always report exactly 0,0 when the Control is in the center). Using the `min` value on a deadzone Processor avoids unintentional input from such Controls. Also, some Controls don't consistently report their maximum values when moving the axis all the way. Using the `max` value on a deadzone Processor ensures that you always get the maximum value in such cases.

## Writing custom Processors

You can also write custom Processors to use in your Project. Custom Processors are available in the UI and code in the same way as the built-in Processors. Add a class derived from [`InputProcessor<TValue>`](../api/UnityEngine.InputSystem.InputProcessor-1.html), and implement the [`Process`](../api/UnityEngine.InputSystem.InputProcessor-1.html#UnityEngine_InputSystem_InputProcessor_1_Process__0_UnityEngine_InputSystem_InputControl_) method:

```CSharp
public class MyValueShiftProcessor : InputProcessor<float>
{
    [Tooltip("Number to add to incoming values.")]
    public float valueShift = 0;

    public override float Process(float value, InputControl control)
    {
        return value + valueShift;
    }
}
```

Now, you need to tell the Input System about your Processor. Call [`InputSystem.RegisterProcessor`](../api/UnityEngine.InputSystem.InputSystem.html#UnityEngine_InputSystem_InputSystem_RegisterProcessor__1_System_String_) in your initialization code. You can do so locally within the Processor class like this:

```CSharp
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class MyValueShiftProcessor : InputProcessor<float>
{
    #if UNITY_EDITOR
    static MyValueShiftProcessor()
    {
        Initialize();
    }
    #endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<MyValueShiftProcessor>();
    }

    //...
}
```

Your new Processor is now available in the [Input Action Asset Editor window](ActionAssets.md), and you can also add it in code like this:

```CSharp
var action = new InputAction(processors: "myvalueshift(valueShift=2.3)");
```

If you want to customize the UI for editing your Processor, create a custom [`InputParameterEditor`](../api/UnityEngine.InputSystem.Editor.InputParameterEditor-1.html) class for it:

```CSharp
// No registration is necessary for an InputParameterEditor.
// The system will automatically find subclasses based on the
// <..> type parameter.
#if UNITY_EDITOR
public class MyValueShiftProcessorEditor : InputParameterEditor<MyValueShiftProcessor>
{
    private GUIContent m_SliderLabel = new GUIContent("Shift By");

    public override void OnEnable()
    {
        // Put initialization code here. Use 'target' to refer
        // to the instance of MyValueShiftProcessor that is being
        // edited.
    }

    public override void OnGUI()
    {
        // Define your custom UI here using EditorGUILayout.
        target.valueShift = EditorGUILayout.Slider(m_SliderLabel,
            target.valueShift, 0, 10);
    }
}
#endif
```
