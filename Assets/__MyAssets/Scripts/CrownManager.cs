
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CrownManager : UdonSharpBehaviour
{
    private VRCPlayerApi[] winners;
    [SerializeField] int numCrowns = 3;

    void Start()
    {
        winners = new VRCPlayerApi[numCrowns];
        for(int i = 0; i < winners.Length; i++)
        {
            winners[i] = null;
        }
    }

    public int SetWinner(int index, VRCPlayerApi winner)
    {
        winners[index] = winner;
        int counter = 0;

        for(int i = 0; i < winners.Length; i++)
        {
            if(Utilities.IsValid(winners[i]))
            {
                if(winners[i] == winner) counter++;
            }
        }

        return counter;
    }
}
