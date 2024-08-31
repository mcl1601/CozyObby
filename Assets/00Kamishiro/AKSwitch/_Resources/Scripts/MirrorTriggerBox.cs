
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Kamishiro.VRChatUDON.AKSwitch
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class MirrorTriggerBox : UdonSharpBehaviour
    {
        [SerializeField] AKSwitch manager;
        private int savedState = 0;
        void Start()
        {
            
        }

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if(player != Networking.LocalPlayer) return;

            manager.State = savedState;
        }
        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if(player != Networking.LocalPlayer) return;
            
            savedState = manager.State;
            manager.State = 0;
        }
    }
}
