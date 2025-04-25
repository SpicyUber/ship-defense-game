using UnityEngine;

public class Cannon : MonoBehaviour
{

    public Ball BallObject;
    [Range(0f, 50f)]
    public float CannonChargeIntensity;

    [Header("~ADDITIONAL ATTRIBUTES~")]

    public GameObject CannonModel;
    public Transform ShootTransform;
    public ParticleSystem ShootParticleSystem;
    private Material _cannonMaterial;
    public bool IsPlayerCannon;
    private Vector3 _startingModelScale;
    private Color _startingColor;
    private float _shootAnimationCooldown = 2f;
    private float _shootAnimationCooldownTimer = 2f;
    private bool _shootAnimationIsOn = false;

    //Intensity variables
    private float _intensityDampener=100f;
    private const float _intensityLimiter=0.5f;

    //Enemy shooting variables
    public Vector3 ShootDirection;
    public Vector3 ShootForce;


    
    
   //test methods
    [ContextMenu("EnemyShoot")]
    void EnemyShotTest() { IsPlayerCannon = false; ShootAfterAnimation(ShootDirection, ShootForce); }

    [ContextMenu("PlayerShoot")]
    void PlayerShotTest() { IsPlayerCannon = true; Shoot(ShootDirection, ShootForce); }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startingModelScale = CannonModel.transform.localScale;
        _cannonMaterial = CannonModel.GetComponent<MeshRenderer>().material;
        _startingColor = _cannonMaterial.GetColor("_Color");
    }

   

    // Update is called once per frame
    void Update()
    {
        ConvertIntensityToVisuals();
        ShootAnimation();
    }

    
    public void ShootAfterAnimation(Vector3 direction, Vector3 force) { 
    
        if(IsPlayerCannon || _shootAnimationCooldownTimer< _shootAnimationCooldown)  return; 

      _shootAnimationIsOn=true;
        _shootAnimationCooldownTimer=0;
        ShootDirection=direction;
        ShootForce=force;
    }

    
    public void Shoot(Vector3 direction, Vector3 force) {
        if (!IsPlayerCannon) { return; }
        ActualShoot("Enemy");
        CannonChargeIntensity = 0;
    }

    private void ActualShoot(string Tag) {
        if(BallObject==null) return;
        ShootExplosion();
        BallObject.Tag = Tag;
        BallObject.BallRigidbody.linearVelocity = Vector3.zero;
        BallObject.ResetLine();
        BallObject.IsCurrentlyFlying = true;
        BallObject.transform.position = ShootTransform.position ;
        BallObject.BallRigidbody.AddForce(ShootForce,ForceMode.VelocityChange);
        
    }

    private void OnDisable()
    {
        CannonChargeIntensity=0;
      
    }

   

    private void ShootAnimation()
    {
        if (!_shootAnimationIsOn) return;


        CannonChargeIntensity = _intensityLimiter * _intensityDampener*(_shootAnimationCooldownTimer / _shootAnimationCooldown);

        _shootAnimationCooldownTimer += Time.deltaTime;
        if(_shootAnimationCooldownTimer >= _shootAnimationCooldown) { CannonChargeIntensity = 0; _shootAnimationIsOn = false; _shootAnimationCooldownTimer = _shootAnimationCooldown; ActualShoot("Player");  }

    }

    private void ShootExplosion()
    { if (ShootParticleSystem == null) return;

    ShootParticleSystem.Play();
        

        

    }

    private void ConvertIntensityToVisuals() { 
       if(CannonModel ==null || _cannonMaterial == null) return;    

        float cannonChargeIntensityAfterDampenerAndLimiter = ((CannonChargeIntensity / _intensityDampener)>_intensityDampener)? _intensityLimiter : (CannonChargeIntensity / _intensityDampener);

        CannonModel.transform.localScale = new( _startingModelScale.x + ( cannonChargeIntensityAfterDampenerAndLimiter*2f),_startingModelScale.y,_startingModelScale.z - (cannonChargeIntensityAfterDampenerAndLimiter / 2f));
        _cannonMaterial.SetColor("_Color", new Color(_startingColor.r+(1f- _startingColor.r)* cannonChargeIntensityAfterDampenerAndLimiter, _startingColor.g - (_startingColor.g) * cannonChargeIntensityAfterDampenerAndLimiter, _startingColor.b - (_startingColor.b) * cannonChargeIntensityAfterDampenerAndLimiter, 1f));


    }
}
