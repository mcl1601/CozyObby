
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PoolLeaderboardSlot : UdonSharpBehaviour
{
    public string displayName;
    public int wins;
    public int losses;
    public string percent;
    public int place;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI lossText;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private GameObject rend;
    [SerializeField] private RectTransform rect;

    public void DeleteSlot()
    {
        Destroy(gameObject);
    }

    public void UpdateSelf()
    {
        nameText.text = displayName;
        winText.text = wins.ToString();
        lossText.text = losses.ToString();
        percentText.text = percent;

        if(place < 11)
        {
            rend.SetActive(true);
            rect.anchoredPosition3D = new Vector3(0f, place * -100f - 80f, 0f);
        }
        else
        {
            rend.SetActive(false);
        }
    }

    
}
