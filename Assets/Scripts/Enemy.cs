using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Cannon CannonObject;
    public float DistanceFromCenter;


    //moje varijable & stuff
    public Transform player;
    public float minDistance = 100f;
    public Rigidbody rb;
    float distance;
    Vector3 movingDirection;
    float movingIntensity = 25f;
    bool _is_on_distance = false;

  
    private void Start()
    {
        transform.LookAt(player);
        //CannonObject.ShootAfterAnimation() // prvi argument normalizovana direkcija, a drugi obicna
    }

    void FixedUpdate()
    {
        distance = Vector3.Distance(player.position, transform.position);
        movingDirection = (player.position - transform.position).normalized;

        //bedan pokusaj raycastinga

        if (Physics.Raycast(transform.position, movingDirection, out RaycastHit hit))
        {
            //stavi ga kod spawnera
            if (hit.collider.tag != "Player")
            {
                _is_on_distance = true;
                rb.linearVelocity = Vector3.zero;
            }
        }

        //provera uslova za kretanje i stizanje u aproksimitet igracu

        if (distance > minDistance && !_is_on_distance)
        {
            rb.AddForce(movingDirection * movingIntensity, ForceMode.Force);
        }
        else if(distance <= minDistance && (_is_on_distance || !_is_on_distance))
        {
            rb.linearVelocity = Vector3.zero;
            _is_on_distance = true;
        }
    }


  

    public void Sink(Vector3 ImpactWorldPosition) { }
    public void SpawnSelf() { }

    public void MoveToRandomLocationOutsidePlayerView() { }

}


