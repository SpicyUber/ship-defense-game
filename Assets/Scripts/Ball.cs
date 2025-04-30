using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody BallRigidbody;
    public bool IsCurrentlyFlying;
    public string Tag;
    private MeshRenderer _ballModel;
    private Collider _ballCollider;
    private TrailRenderer _lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsCurrentlyFlying = false;
        _lineRenderer = GetComponent<TrailRenderer>();
        _ballModel = GetComponentInChildren<MeshRenderer>();
        _ballCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x) > 125f || Mathf.Abs(transform.position.x) > 125f) IsCurrentlyFlying = false;
       _lineRenderer.emitting= IsCurrentlyFlying;
        _ballModel.enabled = IsCurrentlyFlying;
        _ballCollider.enabled = IsCurrentlyFlying;
        if(!IsCurrentlyFlying) { BallRigidbody.linearVelocity = Vector3.zero; }
    }
    private void OnDrawGizmos()
    {
       // Gizmos.DrawCube(Vector3.zero,new(300f,1f,300f));
    }
    private void OnTriggerEnter(Collider other)
    {
        if(Tag != null && Tag.Length!=0 && other.CompareTag(Tag)) {
        IsCurrentlyFlying=false;
        _ballCollider.enabled = IsCurrentlyFlying;
        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip,0.7f);
        if(Tag.Equals("Player")) {

                other.GetComponent<PlayerScript>().MakeExplosion(transform.position);
                FindAnyObjectByType<GameManagerScript>().ScoreDown();
            }else if (Tag.Equals("Enemy")) {

                other.GetComponent<Enemy>().Sink(transform.position);
                other.GetComponent<Enemy>().CannonObject.AbortShot();
                FindAnyObjectByType<GameManagerScript>().ScoreUp();
            }

            FindAnyObjectByType<GameManagerScript>().ShakeCamera(BallRigidbody.linearVelocity,true);


        }
    }

    public void ResetLine()
    {
        if (_lineRenderer == null && _lineRenderer.time==-1) return;
        StartCoroutine(LineRoutine());

    }

    IEnumerator LineRoutine()
    {  _lineRenderer.Clear();
        _lineRenderer.enabled = false;
        _lineRenderer.time = -1;
        yield return new WaitForSeconds(0.125f);
        _lineRenderer.time = 0.5f;
        _lineRenderer.enabled = true;
    }

   

   
}
