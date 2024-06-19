
using System;
using System.Linq;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ObbyManager : UdonSharpBehaviour
{
    #region Variables
    private Checkpoint activeCP;
    private float timer = 0f;
    private float bestTime = -1f;
    private TimeSpan timerSpan;
    private bool isRunning, showingUI;
    [SerializeField] Transform timerObj;
    [SerializeField] GameObject timerCanvas, splitObj, tablets;
    [SerializeField] TextMeshProUGUI timerText, cpText, splitText, finalTime;
    private VRCPlayerApi.TrackingData playerData;
    [SerializeField] Transform[] devTPs;
    [SerializeField] bool useDevTP;
    [SerializeField] Checkpoint[] collectedCPs;
    private int collectedCPCount, activeDevTP = 0;
    public float[] bestTimes, currentRun;
    [SerializeField] TextMeshProUGUI[] uiPlaces;
    private RectTransform hudRect;
    private Vector3 hudInit;
    [SerializeField] float hudAdjustValue = 0.02f;
    [SerializeField] Slider opacitySlider;
    [SerializeField] Image bgImage;
    private AudioSource audioSource;
    [SerializeField] AudioClip start, finish, cpAudio;
    [SerializeField] Crown goldCrown;

    [UdonSynced]
    public string[] leaderboardNames;
    [UdonSynced]
    public float[] leaderboardTimes;
    [UdonSynced]
    public int firstID;
    #endregion

    void Start()
    {
        isRunning = false;
        showingUI = false;
        bestTimes = new float[collectedCPs.Length];
        currentRun = new float[collectedCPs.Length];
        hudRect = timerCanvas.GetComponent<RectTransform>();
        hudInit = hudRect.anchoredPosition;
        for(int i = 0; i < bestTimes.Length; i++)
        {
            bestTimes[i] = -1f;
        }
        leaderboardNames = new string[10];
        leaderboardTimes = new float[10];
        for(int i = 0; i < leaderboardTimes.Length; i++)
        {
            leaderboardTimes[i] = float.MaxValue;
            leaderboardNames[i] = "";
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(showingUI)
        {
            if(Networking.LocalPlayer != null)
            {
                playerData = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
                timerObj.position = playerData.position;
                timerObj.rotation = playerData.rotation;
            }
        }
        if(isRunning)
        {
            timer += Time.deltaTime;
            timerSpan = TimeSpan.FromSeconds(timer);

            timerText.text = FormatTime(timerSpan);

            cpText.text = collectedCPCount.ToString() + " / " + collectedCPs.Length.ToString();
        }
    }

    #region Race Stuff
    private string FormatTime(TimeSpan sp)
    {
        string toReturn = "<mspace=0.55em>" + sp.Minutes.ToString() + "</mspace>:<mspace=0.55em>" + 
            (sp.Seconds > 9 ? sp.Seconds.ToString() : "0" + sp.Seconds.ToString()) + "</mspace>.<mspace=0.55em>" + 
            (sp.Milliseconds > 9 ? (sp.Milliseconds > 99 ? sp.Milliseconds.ToString() : "0" + sp.Milliseconds.ToString()) : "00" + sp.Milliseconds.ToString()) + "</mspace>";

        return toReturn;
    }

    public void RespawnPlayer()
    {
        if(useDevTP)
        {
            Networking.LocalPlayer.TeleportTo(devTPs[activeDevTP].position, devTPs[activeDevTP].rotation);
            Networking.LocalPlayer.SetVelocity(Vector3.zero);
            activeDevTP++;
            activeDevTP %= devTPs.Length;
            return;
        }
        else if(activeCP == null)
        {
            return;
        }
        else
        {
            Networking.LocalPlayer.TeleportTo(activeCP.tran.position, activeCP.tran.rotation);
            Networking.LocalPlayer.SetVelocity(Vector3.zero);
        }

    }

    public void SetActiveCheckpoint(Checkpoint cp)
    {
        if(!isRunning)
        {
            activeCP = cp;
        }

        if(Array.IndexOf(collectedCPs, cp) < 0)
        {
            collectedCPs[collectedCPCount] = cp;

            if(bestTimes[collectedCPCount] < 0)
            {
                currentRun[collectedCPCount] = timer;
            }
            else
            {
                bool plus = bestTimes[collectedCPCount] - timer < 0;
                TimeSpan diff = TimeSpan.FromSeconds(Math.Abs(bestTimes[collectedCPCount] - timer));

                splitText.text = (plus ? "+ " : "- ") + (diff.Minutes == 0 ? "" : diff.Minutes.ToString() + ":") + diff.Seconds.ToString() + "." + (diff.Milliseconds > 9 ? (diff.Milliseconds > 99 ? diff.Milliseconds.ToString() : "0" + diff.Milliseconds.ToString()) : "00" + diff.Milliseconds.ToString());
                splitText.color = bestTimes[collectedCPCount] - timer == 0 ? Color.white : plus ? Color.red : Color.green;

                currentRun[collectedCPCount] = timer;

                splitObj.SetActive(true);
                SendCustomEventDelayedSeconds(nameof(HideSplits), 3f);
            }

            activeCP = cp;
            collectedCPCount++;

            audioSource.clip = cpAudio;
            audioSource.Play();
        }
    }

    public void StartCPHit(Checkpoint cp)
    {
        for(int i = 0; i < collectedCPs.Length; i++)
        {
            collectedCPs[i] = null;
        }
        collectedCPs[0] = cp;
        collectedCPCount = 1;
        activeCP = cp;
        timer = 0f;
        currentRun[0] = 0f;
    }

    public void StartTimer()
    {
        timer = 0f;
        isRunning = true;
        showingUI = true;
        timerCanvas.SetActive(true);
        splitObj.SetActive(false);
        audioSource.clip = start;
        audioSource.Play();
    }

    public void StopTimer()
    {
        if(collectedCPCount == collectedCPs.Length && isRunning)
        {
            isRunning = false;
            showingUI = false;
            timerCanvas.SetActive(false);
            audioSource.clip = finish;
            audioSource.Play();

            if(bestTime == -1f || timer < bestTime)
            {
                bestTimes = currentRun;
                bestTime = timer;

                CheckForLeaderboard();
            }

            finalTime.text = FormatTime(timerSpan);
            tablets.SetActive(true);
        }
    }

    public void ResetTimer()
    {
        isRunning = false;
        timerCanvas.SetActive(false);
        showingUI = false;
    }

    public void HideSplits()
    {
        splitObj.SetActive(false);
    }

    public void TP1() {Networking.LocalPlayer.TeleportTo(devTPs[0].position, devTPs[0].rotation);ResetTimer();}
    public void TP2() {Networking.LocalPlayer.TeleportTo(devTPs[1].position, devTPs[1].rotation);ResetTimer();}
    public void TP3() {Networking.LocalPlayer.TeleportTo(devTPs[2].position, devTPs[2].rotation);ResetTimer();}
    public void TP4() {Networking.LocalPlayer.TeleportTo(devTPs[3].position, devTPs[3].rotation);ResetTimer();}
    public void TP5() {Networking.LocalPlayer.TeleportTo(devTPs[4].position, devTPs[4].rotation);ResetTimer();}
    public void TP6() {Networking.LocalPlayer.TeleportTo(devTPs[5].position, devTPs[5].rotation);ResetTimer();}
    public void TP7() {Networking.LocalPlayer.TeleportTo(devTPs[6].position, devTPs[6].rotation);ResetTimer();}
    public void TP8() {Networking.LocalPlayer.TeleportTo(devTPs[7].position, devTPs[7].rotation);ResetTimer();}
    public void TP9() {Networking.LocalPlayer.TeleportTo(devTPs[8].position, devTPs[8].rotation);ResetTimer();}
    public void TP10() {Networking.LocalPlayer.TeleportTo(devTPs[9].position, devTPs[9].rotation);ResetTimer();}
    public void TP11() {Networking.LocalPlayer.TeleportTo(devTPs[10].position, devTPs[10].rotation);ResetTimer();}
    #endregion

    #region UI Functions
    public void ShowHUD()
    {
        showingUI = true;
        timerCanvas.SetActive(true);
        splitObj.SetActive(true);
    }

    public void HideHUD()
    {
        timerCanvas.SetActive(false);
        splitObj.SetActive(false);
    }

    public void MoveHUDUp()
    {
        hudRect.Translate(0f, hudAdjustValue, 0f);
    }

    public void MoveHUDDown()
    {
        hudRect.Translate(0f, -hudAdjustValue, 0f);
    }

    public void MoveHUDLeft()
    {
        hudRect.Translate(-hudAdjustValue, 0f, 0f);
    }

    public void MoveHUDRight()
    {
        hudRect.Translate(hudAdjustValue, 0f, 0f);
    }

    public void ResetHUD()
    {
        hudRect.anchoredPosition = hudInit;
    }

    public void AdjustBackgroundOpacity()
    {
        Color temp = bgImage.color;
        temp.a = opacitySlider.value;
        bgImage.color = temp;
    }

    public void ScaleHUDUp()
    {
        Vector3 temp = hudRect.localScale;
        temp += new Vector3(0.00002f, 0.00002f, 0.00002f);
        hudRect.localScale = temp;
        hudAdjustValue *= 1.1f;
    }

    public void ScaleHUDDown()
    {
        Vector3 temp = hudRect.localScale;
        temp -= new Vector3(0.00002f, 0.00002f, 0.00002f);
        hudRect.localScale = temp;
        hudAdjustValue *= 0.9f;
    }
    #endregion

    #region Network Stuff
    public override void OnDeserialization()
    {
        // leaderboard updated
        UpdateLeaderboardUI();
        UpdateCrowns();
    }
    private void CheckForLeaderboard()
    {
        // check if the player is on the leaderboard already
        if(UpdateLeaderboard()) return;

        RequestUpdate();
        UpdateLeaderboardUI();
        UpdateCrowns();
    }

    private int GetLeaderboardPosition()
    {
        for(var i = 0; i < leaderboardTimes.Length; i++)
        {
            if(bestTime <= leaderboardTimes[i])
            {
                return i;
            }
        }

        return -1;
    }

    private bool UpdateLeaderboard()
    {
        // check the time on the leaderboard
        int place = GetLeaderboardPosition();

        // didn't make leaderboard
        if(place == -1) return true;

        string previousId = "";
        string newPreviousId = "";
        float previousTime = -1;
        float newPreviousTime = -1;
        bool skipped = false;

        // update the leaderboard arrays
        for(int i = 0; i < leaderboardNames.Length; i++)
        {
            if(i < place) continue;

            if(i == place)
            {
                if(Networking.LocalPlayer.displayName != leaderboardNames[i])
                {
                    // save previous
                    previousId = leaderboardNames[i];
                    previousTime = leaderboardTimes[i];
                }
                else
                {
                    previousId = "";
                }

                // add the player to the leaderboard
                leaderboardNames[i] = Networking.LocalPlayer.displayName;
                leaderboardTimes[i] = bestTime;
            }
            else if(previousId != "")
            {
                if(skipped) break;
                if(Networking.LocalPlayer.displayName == leaderboardNames[i])
                {
                    //move names
                    leaderboardNames[i] = previousId;
                    leaderboardTimes[i] = previousTime;
                    skipped = true;
                }
                else
                {
                    // save previous
                    newPreviousId = leaderboardNames[i];
                    newPreviousTime = leaderboardTimes[i];

                    //move names
                    leaderboardNames[i] = previousId;
                    leaderboardTimes[i] = previousTime;

                    previousId = newPreviousId;
                    previousTime = newPreviousTime;
                }
            }
        }

        // update crowns
        if(place == 0)
        {
            firstID = Networking.LocalPlayer.playerId;
        }
        return false;
    }
    
    private void UpdateLeaderboardUI()
    {
        
        for(int i = 0; i < leaderboardNames.Length; i++)
        {
            if(String.IsNullOrEmpty(leaderboardNames[i])) return;
            uiPlaces[i].text = (i + 1).ToString() + ". <mspace=0.55em>" + FormatTime(TimeSpan.FromSeconds(leaderboardTimes[i])) + "</mspace>     " + leaderboardNames[i];
        }
    }

    private void UpdateCrowns()
    {
        goldCrown.SetPlayer(VRCPlayerApi.GetPlayerById(firstID));
    }

    private void RequestUpdate()
    {
        if(Networking.GetOwner(gameObject) == Networking.LocalPlayer)
        {
            RequestSerialization();
        }  
        else  
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
    }

    public override bool OnOwnershipRequest(VRCPlayerApi requestingPlayer, VRCPlayerApi requestedOwner)
    {
        return true;
    }

    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        if(Networking.LocalPlayer == player)
        {
            RequestSerialization();
        }
    }
    #endregion
}
