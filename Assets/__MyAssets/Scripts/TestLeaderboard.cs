
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TestLeaderboard : UdonSharpBehaviour
{
    [SerializeField] private PoolLeaderboardManager manager;
    private string[] names;
    [SerializeField] private bool FlipWinners;
    void Start()
    {
        names = new string[4];
        names[0] = "Squatch_";
        names[1] = "Chiken!";
        names[2] = "";
        names[3] = "";
    }
    public override void Interact()
    {
        manager.GameResults(FlipWinners ? 1 : 0, names);
    }
}
