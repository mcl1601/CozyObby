
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class Crown : UdonSharpBehaviour
{
    public VRCPlayerApi _player = null;
    private MeshRenderer crown;
    [SerializeField] Vector3 offset;
    
    void Start()
    {
        crown = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if(_player != null)
        {
            transform.position = _player.GetBonePosition(HumanBodyBones.Head) + offset;
        } 
    }

    public void SetPlayer(VRCPlayerApi p)
    {
        _player = p;
        if(p == null)
        {
            crown.enabled = false;
            //crown.gameObject.SetActive(false);
        }
        else
        {
            crown.enabled = true;
            //crown.gameObject.SetActive(true);
        }
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if(player == _player)
        {
            _player = null;
            //crown.gameObject.SetActive(false);
            crown.enabled = false;
        }
    }
}
