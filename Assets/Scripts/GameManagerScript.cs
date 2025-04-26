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
    public CinemachineImpulseSource ImpulseSource;
    public CinemachineBasicMultiChannelPerlin noise;
    private float shakeTimer; // Timer for the shake effect
    public int MaxScore = 100; // Just for testing, this should be set to the max score in the game  
    public int damage = 5; // Just for testing, this should be set to the damage in the game  
    public int hit = 10; // Just for testing, this should be set to the hit in the game  
    public int Score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created  
    void Start()
    {
        ImpulseSource = Camera.GetComponent<CinemachineImpulseSource>();
        noise = Camera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        Score = 100;
        TestShake(Vector3.zero,false);
       
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
     StartCoroutine(ShakeCamera(Vector3.zero, false)); // Test shake effect
    }
    public IEnumerator ShakeCamera(Vector3 dir, bool isExplosion)
    {  
        if (isExplosion)
        {
            float duration = 0.7f;
            noise.AmplitudeGain = 30f;
            noise.FrequencyGain =30f;
            yield return new WaitForSeconds(duration);

            noise.AmplitudeGain = 0;
            noise.FrequencyGain = 0;
        }
        else
        {
           
            ImpulseSource.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Recoil; // Set the impulse shape to Bump                                                                                                   //ImpulseSource.ImpulseDefinition.AmplitudeGain = 10f; // Set the amplitude gain for the impulse signal
            ImpulseSource.ImpulseDefinition.ImpulseDuration = 1.5f;
            ImpulseSource.DefaultVelocity = Vector3.up * 15f; // Set the default velocity for the impulse signal
            ImpulseSource.GenerateImpulse(); // Generate impulse signal
            dir *= -1f;
        }
        }

    }

