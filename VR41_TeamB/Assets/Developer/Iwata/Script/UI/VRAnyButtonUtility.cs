using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;

public static class VRAnyButtonUtility
{
    public static bool AnyPressDown()
    {
        foreach (var device in InputSystem.devices)
        {
            if (device is XRController controller)
            {


                // Primary Button (A / X)
                if (controller.TryGetChildControl<ButtonControl>("primaryButton")?.wasPressedThisFrame == true)
                    return true;

                // Secondary Button (B / Y)
                if (controller.TryGetChildControl<ButtonControl>("secondaryButton")?.wasPressedThisFrame == true)
                    return true;

                // Thumbstick Click
                if (controller.TryGetChildControl<ButtonControl>("thumbstickClicked")?.wasPressedThisFrame == true)
                    return true;

                // Menu（不要なら消してOK）
                if (controller.TryGetChildControl<ButtonControl>("menu")?.wasPressedThisFrame == true)
                    return true;
            }
        }

        return false;
    }
}
