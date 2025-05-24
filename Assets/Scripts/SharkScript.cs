using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SharkScript : MonoBehaviour
{
   public AudioClip Jump,Hurt,Death;
    public GameObject BottledMessage;
    private AudioSource _audioSource;
    private bool _isChasing,_isJumping, _isMunchingPlayer;
    private float _delayBeforeJumpInSeconds=4f;
    private float _delayBeforeChaseInSeconds=1f;
    private float _delayBeforeDeathInSeconds = 16.8f;
    public GameObject SharkModel;
    private float _t = 0;
    private float _goalY = -121f;
    private float _startY;
    private float _startLocalModelY;
    private float _startZ;
    private float _health = 6f;
    public GameObject DeathPanel;
    public ParticleSystem Explosion,Explosion2;
    public ParticleSystem Dust;
    private Coroutine _hurtCoroutine;
    private Material _sharkMaterial;
    private Color _startingColor;
    private Collider _collider;
    private bool _isDead;
    public GameObject SliderPanel;
    public Slider SharkHP;
   
    public CinemachineCamera Main, Side,Last;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _t = 0f;
        _collider = GetComponent<Collider>();
        

         
        _collider.enabled = false;
        _startZ = transform.position.z;
        _startY = transform.position.y;
        _startLocalModelY =SharkModel.transform.localPosition.y;
         _sharkMaterial = SharkModel.GetComponent<MeshRenderer>().material;
        _startingColor = _sharkMaterial.GetColor("_Color");
        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayDelayed(2f);
        StartCoroutine(DelayThenAction(_delayBeforeJumpInSeconds, JumpAction));
        StartCoroutine(DelayThenAction(_delayBeforeDeathInSeconds, PlayerDeathAction));
    }

    public void JumpAction()
    {
        _isJumping = true;
        _collider.enabled = true;
        _t = 0;
        SliderPanel.SetActive(true);
        _audioSource.PlayOneShot(Jump);
    }
    public void PlayerDeathAction()
    { if (_isDead) return;
        _isMunchingPlayer=true;
        GameObject.FindAnyObjectByType<PlayerScript>().enabled = false;
        DeathPanel.SetActive(true);
        StartCoroutine(DelayThenAction(5f, RestartScene));
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void SharkDeath()
    {
        SliderPanel.SetActive(false);
        _audioSource.Stop();
        _audioSource.PlayOneShot(Death);
        GoToSideCam();
        Explosion.Play();
        Explosion2.Play();
        _collider.enabled = false;
        SharkModel.SetActive(false);
        _isChasing = false;
        _isJumping = false;
        _isDead = true;
        StartCoroutine(DelayThenAction(7f, GoToLastCam));

    }

    public void GoToLastCam()
    {
        GameObject.FindFirstObjectByType<CinemachineBrain>().DefaultBlend = new CinemachineBlendDefinition
        {
            Style = CinemachineBlendDefinition.Styles.Linear,
            Time = 1f
        };
        Last.enabled = true;
        Side.enabled = false;
        BottledMessage.SetActive(true);
    }

    public void GoToSideCam()
    {
        GameObject.FindFirstObjectByType<CinemachineBrain>().DefaultBlend = new CinemachineBlendDefinition
        {
            Style = CinemachineBlendDefinition.Styles.Cut,
            Time = 0.1f
        };
        Side.enabled = true;
        Main.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {   if(_isMunchingPlayer) { }
       else if (_isChasing) { SharkModel.transform.localPosition = new(SharkModel.transform.localPosition.x, _startLocalModelY+ Mathf.Sin(_t)*0.01f, SharkModel.transform.localPosition.z); transform.position = new(transform.position.x, transform.position.y,Mathf.Lerp(_startZ,150f,_t/_delayBeforeDeathInSeconds)); if (_t / _delayBeforeDeathInSeconds>=1f) { _isMunchingPlayer = true; _isChasing = false;
                _isJumping = false;
            } }
        else
        if (_isJumping) { transform.position = new(transform.position.x,  Mathf.Lerp(_startY, _goalY, _t / _delayBeforeChaseInSeconds), transform.position.z) ; if (_t / _delayBeforeChaseInSeconds >= 1) { _isChasing = true; _startY = _goalY; _t = 0; Vector3 temp = (-transform.position).normalized; temp.y = 0; } }
        _t += Time.deltaTime;
    }

    IEnumerator DelayThenAction(float delayInSeconds, Action action)
    {
        yield return new WaitForSeconds(delayInSeconds);
        action();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (Dust != null)
            {
                Dust.transform.position = collision.transform.position;
                Dust.Play();
            }
            collision.gameObject.GetComponent<Ball>().IsCurrentlyFlying = false; TakeDamage(); 
           
        }
       
    }

    public void TakeDamage()
    {
        if(_health<=0 || _isMunchingPlayer) return;
        _audioSource.PlayOneShot(Hurt);
        _health--;
        SharkHP.value = _health/6f;
        TurnRed();
        GameObject.FindAnyObjectByType<GameManagerScript>().ShakeCamera(new(1,0,1),true);
        
        if (_hurtCoroutine != null) { StopCoroutine( _hurtCoroutine ); }
        _hurtCoroutine=StartCoroutine(DelayThenAction(0.5f, TurnNormalColor));
        if (_health <= 0)
        {
            SharkDeath();
        }

    }

    public void TurnNormalColor()
    {
        _sharkMaterial.SetColor("_Color", _startingColor);
    }
    public void TurnRed() {
        _sharkMaterial.SetColor("_Color", new Color(_startingColor.r, Mathf.Clamp01( _startingColor.g-0.5f ) , Mathf.Clamp01(_startingColor.b - 0.5f), 1f));
    }


}
