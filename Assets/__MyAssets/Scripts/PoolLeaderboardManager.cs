
using System;
using System.Configuration;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PoolLeaderboardManager : UdonSharpBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private int MAX_LEADERBOARD_SIZE = 40; 
    [SerializeField] private Crown crown;
    private PoolLeaderboardSlot[] leaderboard;
    private bool hasInitialized = false;
    private bool initializedFromSync = false;

    // synced stuff
    [UdonSynced] private string[] syncedNames;
    [UdonSynced] private int[] syncedWins;
    [UdonSynced] private int[] syncedLosses;

    void Start()
    {
        leaderboard = new PoolLeaderboardSlot[MAX_LEADERBOARD_SIZE];
        syncedNames = new string[MAX_LEADERBOARD_SIZE];
        syncedWins = new int[MAX_LEADERBOARD_SIZE];
        syncedLosses = new int[MAX_LEADERBOARD_SIZE];

        for(var i = 0; i < leaderboard.Length; i++)
        {
            leaderboard[i] = null;
            syncedNames[i] = "";
            syncedWins[i] = -1;
            syncedLosses[i] = -1;
        }
    }

    public void GameResults(int winnerId, string[] players)
    {
        if(initializedFromSync)
        {
            initializedFromSync = false;
            return;
        }
        
        hasInitialized = true;
        // loop through players and process them
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i] != "")
            {
                int index = IsPlayerOnLeaderboard(players[i]);
                if(index > -1)
                {
                    // player is on leaderboard
                    if(i == 0 || i == 2)
                    {
                        leaderboard[index].wins += winnerId == 0 ? 1 : 0;
                        leaderboard[index].losses += winnerId == 0 ? 0 : 1;
                    }
                    else
                    {
                        leaderboard[index].wins += winnerId == 1 ? 1 : 0;
                        leaderboard[index].losses += winnerId == 1 ? 0 : 1;
                    }
                    int totalGames = leaderboard[index].wins + leaderboard[index].losses;
                    leaderboard[index].percent = leaderboard[index].losses == 0 ? "100%" : String.Concat(Math.Round((double)leaderboard[index].wins / totalGames * 100), "%");

                    // lost, don't re-sort
                    if(i == 0)
                    {
                        if(winnerId == 1) continue;
                    }
                    else if(winnerId % i == 1) continue;

                    // won, sort leaderboard
                    for(int j = 0; j < leaderboard.Length; j++)
                    {
                        if(j == index) break;

                        if(leaderboard[j].wins < leaderboard[index].wins)
                        {
                            if(index - j == 1)
                            {
                                PoolLeaderboardSlot saved = leaderboard[j];
                                leaderboard[j] = leaderboard[index];
                                leaderboard[index] = saved;
                                break;
                            }
                            else
                            {
                                PoolLeaderboardSlot saved = leaderboard[j];
                                PoolLeaderboardSlot newSaved = null;
                                for(int k = j+1; k < index + 1; k++)
                                {
                                    if(saved == null) break;
                                    newSaved = leaderboard[k];
                                    leaderboard[k] = saved;
                                    saved = newSaved;
                                }
                                leaderboard[j] = leaderboard[index]; break;
                            }
                        }
                    }
                }
                else
                {
                    // player is not on leaderboard
                    // make a new leaderboard slot for them
                    GameObject slotObj = Instantiate(slotPrefab, transform);
                    PoolLeaderboardSlot slot = slotObj.GetComponent<PoolLeaderboardSlot>();
                    slot.displayName = players[i];
                    if(i == 0 || i == 2)
                    {
                        slot.wins = winnerId == 0 ? 1 : 0;
                        slot.losses = winnerId == 0 ? 0 : 1;
                        slot.percent = winnerId == 0 ? "100%" : "0%";
                    }
                    else
                    {
                        slot.wins = winnerId == 1 ? 1 : 0;
                        slot.losses = winnerId == 1 ? 0 : 1;
                        slot.percent = winnerId == 1 ? "100%" : "0%";
                    }
                    
                    // add to leaderboard
                    for(int j = 0; j < leaderboard.Length; j++)
                    {
                        if(leaderboard[j] != null)
                        {
                            if(leaderboard[j].wins < 1)
                            {
                                PoolLeaderboardSlot saved = leaderboard[j];
                                PoolLeaderboardSlot newSaved = null;
                                for(int k = j+1; k < leaderboard.Length; k++)
                                {
                                    if(saved == null) break;
                                    newSaved = leaderboard[k];
                                    leaderboard[k] = saved;
                                    saved = newSaved;
                                }
                                leaderboard[j] = slot; break;
                            }
                        }
                        else
                        {
                            leaderboard[j] = slot; break;
                        }
                    }
                }
            }
        }

        RefreshLeaderboard();
    }

    private int IsPlayerOnLeaderboard(string playerName)
    {
        for(var i = 0; i < leaderboard.Length; i++)
        {
            if(leaderboard[i] == null) return -1;
            if(leaderboard[i].displayName == playerName) return i;
        }

        return -1;
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        int playerIndex = IsPlayerOnLeaderboard(player.displayName);
        if(playerIndex > -1)
        {
            leaderboard[playerIndex].DeleteSlot();
            
            for(int i = playerIndex; i < leaderboard.Length - 1; i++)
            {
                leaderboard[i] = leaderboard[i+1];
            }
            leaderboard[leaderboard.Length - 1] = null;

            RefreshLeaderboard();
        }
    }

    private void RefreshLeaderboard()
    {
        ParseSyncedArrays();

        for(var i = 0; i < leaderboard.Length; i++)
        {
            if(leaderboard[i] != null)
            {
                leaderboard[i].place = i + 1;
                leaderboard[i].UpdateSelf();
            }
        }

        SetCrown();

        if(Networking.IsOwner(gameObject))
        {
            RequestSerialization();
        }
    }

    private void ParseSyncedArrays()
    {
        for(int i = 0; i < leaderboard.Length; i++)
        {
            if(leaderboard[i] == null)
            {
                syncedNames[i] = "";
                syncedWins[i] = -1;
                syncedLosses[i] = -1;
                continue;
            }

            syncedNames[i] = leaderboard[i].displayName;
            syncedWins[i] = leaderboard[i].wins;
            syncedLosses[i] = leaderboard[i].losses;
        }
    }

    private void SetCrown()
    {
        if(leaderboard[0] == null) return;

        VRCPlayerApi[] players = new VRCPlayerApi[MAX_LEADERBOARD_SIZE];
        VRCPlayerApi.GetPlayers(players);
        string first = leaderboard[0].displayName;

        foreach(VRCPlayerApi player in players)
        {
            if(player.displayName == first)
            {
                crown.SetPlayer(player);
                return;
            }
        }
    }

    public override void OnDeserialization()
    {
        if(!hasInitialized)
        {
            for(var i = 0; i < leaderboard.Length; i++)
            {
                if(syncedWins[i] == -1)
                {
                    leaderboard[i] = null;
                    continue;
                }
                
                GameObject slotObj = Instantiate(slotPrefab, transform);
                PoolLeaderboardSlot slot = slotObj.GetComponent<PoolLeaderboardSlot>();
                slot.displayName = syncedNames[i];
                slot.wins = syncedWins[i];
                slot.losses = syncedLosses[i];
                int totalGames = slot.wins + slot.losses;
                slot.percent = slot.losses == 0 ? "100%" : String.Concat(Math.Round((double)slot.wins / totalGames * 100), "%");
                slot.place = i + 1;
                leaderboard[i] = slot;
                slot.UpdateSelf();
            }

            SetCrown();
            
            hasInitialized = true;
            initializedFromSync = true;
        }
    }
}
