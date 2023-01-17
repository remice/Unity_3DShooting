using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool PressJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public static bool PressDescend()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }
}
