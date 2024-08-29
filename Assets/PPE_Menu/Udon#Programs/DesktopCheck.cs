
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DesktopCheck : UdonSharpBehaviour
{
    private UnityEngine.UI.Text text;

    void Start()
    {
        text = this.GetComponent<UnityEngine.UI.Text>(); //Set the "text" variable to be the text component on this object.

        if (Networking.LocalPlayer.IsUserInVR()) // If the local player is in VR,
        {
            text.enabled = false;                // Disable the text.
        }
        else                                     // Otherwise,
        {
            text.enabled = true;                 // enable the text.
        }
    }
}
