using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{
    public GameManagerScript managerScript;
    public PlayerScript player;
    public TextMeshProUGUI Score;
    public Slider Cooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        Score.text= "Score: " + managerScript.Score.ToString();
        Cooldown.value = player.CooldownPercentage();
        
    }
}
