using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManagerScript : MonoBehaviour
{
    public int MaxScore = 100; // Just for testing, this should be set to the max score in the game  
    public int damage = 5; // Just for testing, this should be set to the damage in the game  
    public int hit = 10; // Just for testing, this should be set to the hit in the game  
    public int Score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created  
    void Start()
    {
        Score = 5;
    }

    // Update is called once per frame  
    void Update()
    {
        ScoreDown();// Just to test if the function is working
    }

    public void ScoreUp()
    {
        Score = Mathf.Min(100, Score + hit); // Adding score
              
        }

    public void ScoreDown()
    {
        Score = Mathf.Max(0, Score - damage);// Decrease score 

    
    }

    public void ShakeCamera(Vector3 dir, bool isExplosion) { } // isExplosion true means Explosion, otherwise Recoil  
}
