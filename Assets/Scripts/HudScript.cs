using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{
    public GameManagerScript managerScript;
    public PlayerScript player;
    public Color ScoreUpColor;
    public Color ScoreDownColor;
    public TextMeshProUGUI Score;
    public Slider Cooldown;
    private Vector3 textStartScale;
    private Color textStartColor;
    private Quaternion textStartRotation;
    private Vector2 textStartPosition;
    private Coroutine _coroutine;
    private float animationSpeed = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       if(Score!= null)
        {
            textStartScale = Score.transform.localScale;
            textStartColor = Score.color;
            textStartRotation = Score.rectTransform.localRotation;
            textStartPosition = Score.rectTransform.localPosition;
        }


    }

    // Update is called once per frame
    void Update()
    {
        Score.text= "Score: " + managerScript.Score.ToString();
        Cooldown.value = player.CooldownPercentage();
    }

   public void ScoreUp() { if (_coroutine != null) { StopCoroutine(_coroutine); } _coroutine = StartCoroutine(ScoreAnimation(true)); }

  public void  ScoreDown() { if (_coroutine != null) { StopCoroutine(_coroutine); } _coroutine = StartCoroutine(ScoreAnimation(false)); }

    IEnumerator ScoreAnimation(bool scoreWentUp)
    {
        float t = 0f;
        Score.rectTransform.localScale = textStartScale;
        Score.color = textStartColor;
        Score.rectTransform.rotation = textStartRotation;
        Score.rectTransform.localPosition = textStartPosition;
        if (scoreWentUp) {
            Score.color = ScoreUpColor;
            while (t<animationSpeed/2.5f) {
                Score.rectTransform.localScale = textStartScale*(1f+(t/(animationSpeed/2.5f)*0.1f));
                yield return null;
                t += Time.deltaTime;
            }
            t = 0f;
            Vector3 tempScale = textStartScale * 1.1f;
            while (t + animationSpeed / 2.5f < animationSpeed)
            {
                Score.rectTransform.localScale= Vector3.Lerp(tempScale,textStartScale, (t + animationSpeed / 2.5f) / animationSpeed);
                yield return null;
                t += Time.deltaTime;
            }

        } else
        {
            Score.color = ScoreDownColor;
            Vector2 firstLocation = textStartPosition + new Vector2(5f, 5f);
            Vector2 secondLocation = textStartPosition - new Vector2(5f, 5f);
            while ((t < animationSpeed / 6f))
            {
              Score.rectTransform.localPosition=  Vector2.Lerp(textStartPosition,firstLocation, t / (animationSpeed / 4f));
                yield return null;
                t += Time.deltaTime;
            }
            t = 0;
             
            while ((t < animationSpeed / 6f))
            {
                Score.rectTransform.localPosition = Vector2.Lerp(firstLocation, secondLocation, t / (animationSpeed / 4f));

                yield return null;
                t += Time.deltaTime;
            }
            t = 0;
              
            while ((t < animationSpeed / 6f))
            {
                Score.rectTransform.localPosition = Vector2.Lerp(secondLocation, firstLocation , t / (animationSpeed / 4f));
                yield return null;
                t += Time.deltaTime;
            }
            t = 0;
            while ((t < animationSpeed / 6f))
            {
                Score.rectTransform.localPosition = Vector2.Lerp(firstLocation, secondLocation, t / (animationSpeed / 4f));

                yield return null;
                t += Time.deltaTime;
            }
            t = 0;

            while ((t < animationSpeed / 6f))
            {
                Score.rectTransform.localPosition = Vector2.Lerp(secondLocation, firstLocation, t / (animationSpeed / 4f));
                yield return null;
                t += Time.deltaTime;
            }
            t = 0;

            while ((t < animationSpeed / 6f))
            {
                Score.rectTransform.localPosition = Vector2.Lerp(firstLocation,textStartPosition , t / (animationSpeed / 4f));

                yield return null;
                t += Time.deltaTime;
            }
            
        }
        Score.rectTransform.localScale = textStartScale;
        Score.color = textStartColor;
        Score.rectTransform.rotation = textStartRotation;
        Score.rectTransform.localPosition = textStartPosition;
        yield return null;

    }
}
