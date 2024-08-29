
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ToggleAll : UdonSharpBehaviour
{
    [SerializeField] private GameObject volumesParent;
    [SerializeField] private Animator menuAnimator;

    public void OnClick()
    {
        volumesParent.SetActive(!volumesParent.activeSelf);

        if (volumesParent.activeSelf)
        {
            menuAnimator.SetBool("on", true);
        }
        else
        {
            menuAnimator.SetBool("on", false);
        }
    }
}
