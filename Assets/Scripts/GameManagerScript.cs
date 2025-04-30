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
    public AudioSource MusicSource;
    public CinemachineImpulseSource ImpulseSource;
    public CinemachineBasicMultiChannelPerlin noise;
    private float shakeTimer; // Timer for the shake effect
    public int MaxScore = 100; // Just for testing, this should be set to the max score in the game  
    public int damage = 5; // Just for testing, this should be set to the damage in the game  
    public int hit = 10; // Just for testing, this should be set to the hit in the game  
    public int Score;
    private HudScript hud;
    private bool sharkEvent = false;
    public GameObject Shark;

    // Start is called once before the first execution of Update after the MonoBehaviour is created  
    void Start()
    {
         hud = GameObject.FindAnyObjectByType<HudScript>();
       // Score = 99;
        //ShakeCamera(Vector3.zero,false); Testing Shake
       
    }
 

    // Update is called once per frame  
    void Update()
    { 
       // ScoreDown();// Just to test if the function is working
    }
    [ContextMenu("Test Score")]
    public void ScoreUp()
    {
        if (sharkEvent) return;

        if (Score != MaxScore) hud.ScoreUp();
        Score = Mathf.Min(MaxScore, Score + hit); // Adding score
        if(Score==MaxScore && !sharkEvent) { SharkEvent(); }
    }
    public void SharkEvent()
    {if(sharkEvent) { return; }
        sharkEvent = true;
        GameObject.FindFirstObjectByType<EnemySpawner>().SharkEvent();
        Shark.SetActive(true);
        StartCoroutine(SharkCameraShake());
        GetComponent<AudioSource>().Stop();
    }

  IEnumerator SharkCameraShake() { yield return new WaitForSeconds(4f); ShakeCamera(new(50f, 0, 50f), true); }
    public void ScoreDown()
    { if (sharkEvent) return;
        Score = Mathf.Max(0, Score - damage);// Decrease score 
        hud.ScoreDown();

    }
    public void ShakeCamera(Vector3 dir, bool isExplosion)
    {
        
     StartCoroutine(TestShake(dir, isExplosion)); // Test shake effect
    }
    public IEnumerator TestShake(Vector3 dir, bool isExplosion)
    {
       /* if (noise == null)
            Debug.LogError("Noise is NULL!");

        if (ImpulseSource == null)
            Debug.LogError("ImpulseSource is NULL!");

        if (ImpulseSource != null && ImpulseSource.ImpulseDefinition == null)
            Debug.LogError("ImpulseDefinition is NULL!");*/

        if (isExplosion)
        {
            Explode1(dir);
            yield return new WaitForSeconds(0.7f);
            Explode2(dir);
        }
        else
        {

            Recoil(dir);

        }

       
        }

    private void Explode1(Vector3 dir) {
        
        noise.AmplitudeGain = 30f;
        noise.FrequencyGain = 30f;
       

        
         
    }

    private void Explode2(Vector3 dir) {
        noise.AmplitudeGain = 0;
        noise.FrequencyGain = 0;
    }
    private void Recoil(Vector3 dir) {
        ImpulseSource.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Recoil; // Set the impulse shape to Bump                                                                                                   //ImpulseSource.ImpulseDefinition.AmplitudeGain = 10f; // Set the amplitude gain for the impulse signal
        ImpulseSource.ImpulseDefinition.ImpulseDuration = 1.5f;
        dir *= -1f;
        ImpulseSource.DefaultVelocity = dir; // Set the default velocity for the impulse signal
        ImpulseSource.GenerateImpulseWithForce(15f); // Generate impulse signal
        
    }

}

