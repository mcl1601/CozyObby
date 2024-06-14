
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DayNiteManager : UdonSharpBehaviour
{
    [SerializeField] bool doCycle;
    [SerializeField] Animator anim;
    [SerializeField] float speed = 0.1f;
    void Start()
    {
        
    }

    public override void Interact()
    {
        doCycle = !doCycle;
        anim.SetFloat("Speed", doCycle ? speed : 0);
    }
}
