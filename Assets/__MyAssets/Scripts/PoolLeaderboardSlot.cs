
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
        nameText.text = FormatName(displayName);
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

    private string FormatName(string input)
    {
        if(input == "Squatch_") {return rainbow(input);}
        if(input == "Chikenǃ") {return $"<color=#6c9fce>{input}</color>";}
        if(input == "Kat-") {return $"<color=#fdb8d1>{input}</color>";}
        if(input == "~Can~") {return $"<color=#e681e5>{name}</color>";}
        if(input == "~Hayashi~") {return $"<color=#3c8f43>{name}</color>";}

        return input;
    }

    private string rainbow(string name)
    {
        string[] colors = generateRainbow(name.Length);
        for (int i = 0; i < name.Length; i++)
        {
            colors[i] = $"<color=#{colors[i]}>{name[i]}</color>";
        }
        return string.Join("", colors);
    }

    private string[] generateRainbow(int numColors)
    {
        string[] colors = new string[numColors];

        float n = (float)numColors;
        for(int i = 0; i < numColors; i++)
        {
            int red = 255;
            int green = 0;
            int blue = 0;
            //red: (first quarter)
            if (i <= n / 4)
            {
                red = 255;
                green = (int)(255 / (n / 4) * i);
                blue = 0;
            }
            else if (i <= n / 2)  //2nd quarter
            {
                red = (int)((-255)/(n/4)*i + 255 * 2);
                green = 255;
                blue = 0;
            }
            else if (i <= (.75)*n)
            { // 3rd quarter
                red = 0;
                green = 255;
                blue = (int)(255 / (n / 4) * i + (-255 * 2));
            }
            else if(i > (.75)*n)
            {
                red = 0;
                green = (int)(-255 * i / (n / 4) + (255 * 4));
                blue = 255;
            }
            
            colors[i] = $"{red.ToString("X2")}{green.ToString("X2")}{blue.ToString("X2")}";
        }
        return colors;
    }
}
