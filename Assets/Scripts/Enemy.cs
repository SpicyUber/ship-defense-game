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
    Vector3 movingDirectionNormalized;
    float movingIntensity = 25f;
    bool _is_on_distance = false;
    float timer = 0;
    public float interval;
    bool _timer_currently_active = false;
    int firingChances;



    private void Start()
    {
        transform.LookAt(player);
        interval = Random.Range(5, 10);
        //CannonObject.ShootAfterAnimation() // prvi argument normalizovana direkcija, a drugi obicna
    }

    void FixedUpdate()
    {
        distance = Vector3.Distance(player.position, transform.position);
        movingDirection = (player.position - transform.position);
        movingDirectionNormalized = movingDirection.normalized;

        if(_timer_currently_active && timer < interval)
        {
            timer += Time.fixedDeltaTime;
        }

        if(_timer_currently_active && timer >= interval)
        {
            
            firingChances = Random.Range(1, 3);
            //ako je firing chances 1, top zapuca, a ako je 2, ne zapuca; u oba slucaja se ponovo bira random interval i ponovo zapocinje tajmer

            if (firingChances == 1)
            {
                CannonObject.ShootAfterAnimation(movingDirectionNormalized, movingDirection);
                timer = 0f;
                interval = Random.Range(5, 10);
            }

            else if (firingChances == 2)
            {
                timer = 0f;
                interval = Random.Range(5, 10);
            }
        }



        //bedan pokusaj raycastinga

        if (Physics.Raycast(transform.position, movingDirectionNormalized, out RaycastHit hit))
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
            rb.AddForce(movingDirectionNormalized * movingIntensity, ForceMode.Force);
        }
        else if(distance <= minDistance && (_is_on_distance || !_is_on_distance))
        {
            rb.linearVelocity = Vector3.zero;
            _is_on_distance = true;
            //pokrece se tajmer za cannon
            _timer_currently_active = true;
        }
    }
  

    public void Sink(Vector3 ImpactWorldPosition) { }
    public void SpawnSelf() { }

    public void MoveToRandomLocationOutsidePlayerView() { }

}


