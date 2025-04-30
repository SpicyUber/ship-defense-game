using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IntroTutorial : MonoBehaviour
{
    public Behaviour[] BehavioursToActivate;
    public Sprite TapToStart;
    public CinemachineCamera MainCamera;
    public CinemachineCamera IntroCamera;
    private bool _gameStarted;
    private bool _readyToStart;
    private float _touchStartTime = 0f;
    private float _quickTapThreshold = 0.2f;
    public EnemySpawner Spawner;

    void Update()
    {
        var touch = Touchscreen.current?.primaryTouch;

        if (touch == null)
            return;

        if (touch.press.wasPressedThisFrame)
        {
            _touchStartTime = Time.time;
        }

        if (touch.press.wasReleasedThisFrame && _readyToStart)
        {
            float touchDuration = Time.time - _touchStartTime;
            if (touchDuration <= _quickTapThreshold && _readyToStart)
            {
                StartGame();
            }
        }
    }
    void Start()
    {
        foreach (Behaviour go in BehavioursToActivate) { if (go != null) { go.enabled=false; } }
        MainCamera.enabled = false;
        IntroCamera.enabled = true;
        StartCoroutine(NextImage());


    }

     
    IEnumerator NextImage()
    {
        yield return new WaitForSeconds(5f);
        GetComponent<Image>().sprite = TapToStart;
        _readyToStart = true;
    }
    IEnumerator DestroyAtEndOfFrame( )
    {


        yield return new WaitForEndOfFrame();
        foreach (Behaviour go in BehavioursToActivate) { if (go != null) { go.enabled = true; } }
        MainCamera.enabled = true;
        IntroCamera.enabled = false;
        Destroy(this.gameObject);
    }

   private void StartGame() { if (_gameStarted) return; Spawner.enabled = true; StartCoroutine(DestroyAtEndOfFrame()); }
}
