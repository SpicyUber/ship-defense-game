using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;

public class PlayerScript : MonoBehaviour
{
    public HudScript HUD;
    public AudioSource cooldownCompleteAudio;
    public GameManagerScript gameManager;
    public Cannon CannonObject;
    private Vector2 _touchPosition;
    public DragVisual Visual;
    public bool IsPressed;
    private Vector3 touchStart;
    private Vector3 touchEnd;
    public float PredefinedLengthFromCamera = 400;
    public float cooldownTime = 3;
    public float cooldownTimer = 0;
    public Boolean IsCooldown = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindObjectOfType<GameManagerScript>();
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();

        _touchPosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 TouchScreenPosition = Touchscreen.current.primaryTouch.position.value;
        TouchScreenPosition.z = PredefinedLengthFromCamera;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(TouchScreenPosition);
     
        HandleCooldown();        
        
        //prvi dodir
        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            if (!IsCooldown)
            {
                Vector2 playerXZ = new Vector2(transform.position.x, transform.position.z);
                Vector2 touchXZ = new Vector2(worldPos.x, worldPos.z);
                float distance = Vector2.Distance(playerXZ, touchXZ);

                if (distance < 20f) // prag koji definiše "blizu"
                {
                    IsPressed = true;
                    touchStart = worldPos;
                    Visual.FirstPoint = touchStart;
                    Visual.SecondPoint = touchStart;
                    Visual.IsEnabled = true;
                }
            }
        }
        //prst se drzi na ekranu
        if (Touchscreen.current.primaryTouch.press.isPressed && IsPressed && !IsCooldown)
        {
            Visual.SecondPoint = worldPos;

            // rotacija broda
            Vector3 direction = (touchStart - worldPos).normalized;
            direction.y = 0f; // da ne rotira gore/dole

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }

        //prst podignut sa ekrana
        else
        if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
        {

            if (!IsCooldown && IsPressed)
            {

                IsPressed = false;
                touchEnd = Camera.main.ScreenToWorldPoint(TouchScreenPosition);
                CannonObject.CannonChargeIntensity = (touchStart - touchEnd).magnitude;
                Visual.IsEnabled = false;
                Vector3 direction = (touchStart - touchEnd).normalized;
                Vector3 force = touchStart - touchEnd;                
                CannonObject.Shoot(direction, force);
                
                if (gameManager != null)
                {
                    gameManager.ShakeCamera(-direction, false);
                    //Debug.LogError("GameManagerScript is not found in the scene!");
                }

                //FindFirstObjectByType<GameManagerScript>().ShakeCamera(direction, false);

                IsCooldown = true;
                cooldownTimer = cooldownTime;
                
            } 
            Visual.IsEnabled = false;            
        }
    }

    private void HandleCooldown()
    {
        if (IsCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                IsCooldown = false;
                cooldownTimer = 0f;
                if (cooldownCompleteAudio != null)
                {
                    cooldownCompleteAudio.Play();
                }
            }
        }
    }
    public void MakeExplosion(Vector3 ImpactWorldPosition)
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ps.transform.position = ImpactWorldPosition;
            ps.Play();
        }
    }
    public float CooldownPercentage()
    {
        float percent = 1 - cooldownTimer / cooldownTime;
        if (!IsCooldown) return 100f;
        return (1f - cooldownTimer / cooldownTime) * 100f;        
    } 


}