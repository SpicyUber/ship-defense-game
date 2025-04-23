using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManagerScript : MonoBehaviour
{
    public int Score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScoreUp() { }
    public void ScoreDown() { }

    public void ShakeCamera(Vector3 dir, bool isExplosion) { } //isExplosion true znaci da je Explosion, u suprotnom Recoil
}
