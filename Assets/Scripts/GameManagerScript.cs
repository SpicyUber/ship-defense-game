using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using Unity.Cinemachine;
using Unity.VisualScripting;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{
    public CinemachineCamera Camera;
    public CinemachineBasicMultiChannelPerlin noise;
    private float shakeTimer; // Timer for the shake effect
    public int MaxScore = 100; // Just for testing, this should be set to the max score in the game  
    public int damage = 5; // Just for testing, this should be set to the damage in the game  
    public int hit = 10; // Just for testing, this should be set to the hit in the game  
    public int Score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created  
    void Start()
    {
        noise = Camera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        Score = 100;
        TestShake(Vector3.zero,true);
    }

    // Update is called once per frame  
    void Update()
    {
        ScoreDown();// Just to test if the function is working
    }
    [ContextMenu("Test Score")]
    public void ScoreUp()
    {
        Score = Mathf.Min(100, Score + hit); // Adding score

    }

    public void ScoreDown()
    {
        Score = Mathf.Max(0, Score - damage);// Decrease score 


    }
    private void TestShake(Vector3 dir, bool isExplosion)
    {
     StartCoroutine(ShakeCamera(Vector3.zero, true)); // Test shake effect
    }
    public IEnumerator ShakeCamera(Vector3 dir, bool isExplosion)
    {  // isExplosion true means Explosion, otherwise Recoil  



        //float amplitude = isExplosion ? 3f : 1f;
        //float frequency = isExplosion ? 2f : 1f;
        //float duration = isExplosion ? 3f : 0.2f;
        if (isExplosion)
        {
            float duration = 0.5f;
            noise.AmplitudeGain = 80f;
            noise.FrequencyGain = 0.6f;
            yield return new WaitForSeconds(duration);

            noise.AmplitudeGain = 0;
            noise.FrequencyGain = 0;
        }
        else
        {
            dir *= -1f;
        }
        }

    }

