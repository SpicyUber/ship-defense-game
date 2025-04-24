using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{
    public PlayerScript player;
    public TextMeshProUGUI Score;
    public Slider Cooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cooldown.Value = player.CooldownPercentage();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
