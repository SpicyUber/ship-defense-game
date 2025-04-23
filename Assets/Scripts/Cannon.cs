using UnityEngine;

public class Cannon : MonoBehaviour
{

    public Ball BallObject;
    [Range(0f, 50f)]
    public float CannonChargeIntensity;

    [Header("~ADDITIONAL ATTRIBUTES~")]

    public GameObject CannonModel;
    private Material _cannonMaterial;
    public bool IsPlayerCannon;
    private Vector3 _startingModelScale;
    private Color _startingColor;

    //Intensity variables
    private float _intensityDampener=100f;
    private const float _intensityLimiter=0.5f;

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
    }

    public void ShootAfterAnimation(Vector3 direction, Vector3 force) { }

    public void Shoot(Vector3 direction, Vector3 force) { }

    private void ConvertIntensityToVisuals() { 
        if (!IsPlayerCannon) { return; }

        float cannonChargeIntensityAfterDampenerAndLimiter = ((CannonChargeIntensity / _intensityDampener)>_intensityDampener)? _intensityLimiter : (CannonChargeIntensity / _intensityDampener);

        CannonModel.transform.localScale = new( _startingModelScale.x + ( cannonChargeIntensityAfterDampenerAndLimiter*2f),_startingModelScale.y,_startingModelScale.z - (cannonChargeIntensityAfterDampenerAndLimiter / 2f));
        _cannonMaterial.SetColor("_Color", new Color(_startingColor.r+(1f- _startingColor.r)* cannonChargeIntensityAfterDampenerAndLimiter, _startingColor.g - (_startingColor.g) * cannonChargeIntensityAfterDampenerAndLimiter, _startingColor.b - (_startingColor.b) * cannonChargeIntensityAfterDampenerAndLimiter, 1f));


    }
}
