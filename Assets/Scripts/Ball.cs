using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody BallRigidbody;
    public bool IsCurrentlyFlying;
    public string Tag;
    private MeshRenderer _ballModel;
    private Collider _ballCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsCurrentlyFlying = false;
        _ballModel = GetComponentInChildren<MeshRenderer>();
        _ballCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
       
        _ballModel.enabled = IsCurrentlyFlying;
        _ballCollider.enabled = IsCurrentlyFlying;
    }
}
