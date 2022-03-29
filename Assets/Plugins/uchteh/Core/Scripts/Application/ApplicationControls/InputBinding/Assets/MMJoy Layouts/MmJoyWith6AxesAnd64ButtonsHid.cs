using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

[StructLayout(LayoutKind.Explicit, Size = 64)]
internal struct MMJoyWith6AxesAnd64ButtonsInputReport : IInputStateTypeInfo
{

    // Because all HID input reports are tagged with the 'HID ' FourCC,
    // this is the format we need to use for this state struct.
    public FourCC format => new FourCC('H', 'I', 'D');
    [FieldOffset(0)] public byte reportId;

    [InputControl(layout = "Axis", format = "BYTE")] [FieldOffset(1)] public readonly byte axis1;
    [InputControl(layout = "Axis", format = "BYTE")] [FieldOffset(2)] public readonly byte axis2;
    [InputControl(layout = "Axis", format = "BYTE")] [FieldOffset(3)] public readonly byte axis3;
    [InputControl(layout = "Axis", format = "BYTE")] [FieldOffset(4)] public readonly byte axis4;
    [InputControl(layout = "Axis", format = "BYTE")] [FieldOffset(5)] public readonly byte axis5;
    [InputControl(layout = "Axis", format = "BYTE")] [FieldOffset(6)] public readonly byte axis6;

    [InputControl(name = "Button01", layout = "Button", format = "BIT", displayName = "Button", bit = 0)]
    [InputControl(name = "Button02", layout = "Button", format = "BIT", displayName = "Button", bit = 1)]
    [InputControl(name = "Button03", layout = "Button", format = "BIT", displayName = "Button", bit = 2)]
    [InputControl(name = "Button04", layout = "Button", format = "BIT", displayName = "Button", bit = 3)]
    [InputControl(name = "Button05", layout = "Button", format = "BIT", displayName = "Button", bit = 4)]
    [InputControl(name = "Button06", layout = "Button", format = "BIT", displayName = "Button", bit = 5)]
    [InputControl(name = "Button07", layout = "Button", format = "BIT", displayName = "Button", bit = 6)]
    [InputControl(name = "Button08", layout = "Button", format = "BIT", displayName = "Button", bit = 7)]
    [InputControl(name = "Button09", layout = "Button", format = "BIT", displayName = "Button", bit = 8)]
    [InputControl(name = "Button10", layout = "Button", format = "BIT", displayName = "Button", bit = 9)]
    [InputControl(name = "Button11", layout = "Button", format = "BIT", displayName = "Button", bit = 10)]
    [InputControl(name = "Button12", layout = "Button", format = "BIT", displayName = "Button", bit = 11)]
    [InputControl(name = "Button13", layout = "Button", format = "BIT", displayName = "Button", bit = 12)]
    [InputControl(name = "Button14", layout = "Button", format = "BIT", displayName = "Button", bit = 13)]
    [InputControl(name = "Button15", layout = "Button", format = "BIT", displayName = "Button", bit = 14)]
    [InputControl(name = "Button16", layout = "Button", format = "BIT", displayName = "Button", bit = 15)]
    [InputControl(name = "Button17", layout = "Button", format = "BIT", displayName = "Button", bit = 16)]
    [InputControl(name = "Button18", layout = "Button", format = "BIT", displayName = "Button", bit = 17)]
    [InputControl(name = "Button19", layout = "Button", format = "BIT", displayName = "Button", bit = 18)]
    [InputControl(name = "Button20", layout = "Button", format = "BIT", displayName = "Button", bit = 19)]
    [InputControl(name = "Button21", layout = "Button", format = "BIT", displayName = "Button", bit = 20)]
    [InputControl(name = "Button22", layout = "Button", format = "BIT", displayName = "Button", bit = 21)]
    [InputControl(name = "Button23", layout = "Button", format = "BIT", displayName = "Button", bit = 22)]
    [InputControl(name = "Button24", layout = "Button", format = "BIT", displayName = "Button", bit = 23)]
    [InputControl(name = "Button25", layout = "Button", format = "BIT", displayName = "Button", bit = 24)]
    [InputControl(name = "Button26", layout = "Button", format = "BIT", displayName = "Button", bit = 25)]
    [InputControl(name = "Button27", layout = "Button", format = "BIT", displayName = "Button", bit = 26)]
    [InputControl(name = "Button28", layout = "Button", format = "BIT", displayName = "Button", bit = 27)]
    [InputControl(name = "Button29", layout = "Button", format = "BIT", displayName = "Button", bit = 28)]
    [InputControl(name = "Button30", layout = "Button", format = "BIT", displayName = "Button", bit = 29)]
    [InputControl(name = "Button31", layout = "Button", format = "BIT", displayName = "Button", bit = 30)]
    [InputControl(name = "Button32", layout = "Button", format = "BIT", displayName = "Button", bit = 31)]
    [InputControl(name = "Button33", layout = "Button", format = "BIT", displayName = "Button", bit = 32)]
    [InputControl(name = "Button34", layout = "Button", format = "BIT", displayName = "Button", bit = 33)]
    [InputControl(name = "Button35", layout = "Button", format = "BIT", displayName = "Button", bit = 34)]
    [InputControl(name = "Button36", layout = "Button", format = "BIT", displayName = "Button", bit = 35)]
    [InputControl(name = "Button37", layout = "Button", format = "BIT", displayName = "Button", bit = 36)]
    [InputControl(name = "Button38", layout = "Button", format = "BIT", displayName = "Button", bit = 37)]
    [InputControl(name = "Button39", layout = "Button", format = "BIT", displayName = "Button", bit = 38)]
    [InputControl(name = "Button40", layout = "Button", format = "BIT", displayName = "Button", bit = 39)]
    [InputControl(name = "Button41", layout = "Button", format = "BIT", displayName = "Button", bit = 40)]
    [InputControl(name = "Button42", layout = "Button", format = "BIT", displayName = "Button", bit = 41)]
    [InputControl(name = "Button43", layout = "Button", format = "BIT", displayName = "Button", bit = 42)]
    [InputControl(name = "Button44", layout = "Button", format = "BIT", displayName = "Button", bit = 43)]
    [InputControl(name = "Button45", layout = "Button", format = "BIT", displayName = "Button", bit = 44)]
    [InputControl(name = "Button46", layout = "Button", format = "BIT", displayName = "Button", bit = 45)]
    [InputControl(name = "Button47", layout = "Button", format = "BIT", displayName = "Button", bit = 46)]
    [InputControl(name = "Button48", layout = "Button", format = "BIT", displayName = "Button", bit = 47)]
    [InputControl(name = "Button49", layout = "Button", format = "BIT", displayName = "Button", bit = 48)]
    [InputControl(name = "Button50", layout = "Button", format = "BIT", displayName = "Button", bit = 49)]
    [InputControl(name = "Button51", layout = "Button", format = "BIT", displayName = "Button", bit = 50)]
    [InputControl(name = "Button52", layout = "Button", format = "BIT", displayName = "Button", bit = 51)]
    [InputControl(name = "Button53", layout = "Button", format = "BIT", displayName = "Button", bit = 52)]
    [InputControl(name = "Button54", layout = "Button", format = "BIT", displayName = "Button", bit = 53)]
    [InputControl(name = "Button55", layout = "Button", format = "BIT", displayName = "Button", bit = 54)]
    [InputControl(name = "Button56", layout = "Button", format = "BIT", displayName = "Button", bit = 55)]
    [InputControl(name = "Button57", layout = "Button", format = "BIT", displayName = "Button", bit = 56)]
    [InputControl(name = "Button58", layout = "Button", format = "BIT", displayName = "Button", bit = 57)]
    [InputControl(name = "Button59", layout = "Button", format = "BIT", displayName = "Button", bit = 58)]
    [InputControl(name = "Button60", layout = "Button", format = "BIT", displayName = "Button", bit = 59)]
    [InputControl(name = "Button61", layout = "Button", format = "BIT", displayName = "Button", bit = 60)]
    [InputControl(name = "Button62", layout = "Button", format = "BIT", displayName = "Button", bit = 61)]
    [InputControl(name = "Button63", layout = "Button", format = "BIT", displayName = "Button", bit = 62)]
    [InputControl(name = "Button64", layout = "Button", format = "BIT", displayName = "Button", bit = 63)]
    [FieldOffset(7)]
    public readonly byte buttons;
}


[InputControlLayout(stateType = typeof(MMJoyWith6AxesAnd64ButtonsInputReport))]
#if UNITY_EDITOR
[InitializeOnLoad] // Make sure static constructor is called during startup.
#endif
public class MmJoyWith6AxesAnd64ButtonsHid : InputDevice
{

    static MmJoyWith6AxesAnd64ButtonsHid()
    {
	    // This is way to match the Device.
        InputSystem.RegisterLayout<MmJoyWith6AxesAnd64ButtonsHid>(
            matches: new InputDeviceMatcher()
                .WithInterface("HID")
                .WithProduct("MMJoy6Axs64Btn")
                .WithCapability("vendorId", 0x8886) // Sony Entertainment.
                .WithCapability("productId", 0x8886) // Wireless controller.
                .WithCapability("usage", 0x0004)); // Wireless controller.
    }

    // In the Player, to trigger the calling of the static constructor,
    // create an empty method annotated with RuntimeInitializeOnLoadMethod.
    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {


    }
}
