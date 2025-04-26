using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Cannon CannonObject;
    public float DistanceFromCenter;

    public Transform explosion;
    public ParticleSystem explosion_particle;
    public RockUpAndDown RockUpAndDown;
    public GameObject EnemyChildren;

    //moje varijable & stuff
    public Transform player;
    public BoxCollider boxCollider;
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
    bool _should_enemy_sink = false;
    bool _enemy_sank = false;
    float spawnTimer;
    float spawnInterval;




    private void Start()
    {
        transform.LookAt(player);
        interval = Random.Range(5, 10);
        spawnTimer = 0;
    }

    void FixedUpdate()
    {
        distance = Vector3.Distance(player.position, transform.position);
        movingDirection = (player.position - transform.position);
        movingDirectionNormalized = movingDirection.normalized;

        if(_should_enemy_sink && !_enemy_sank)
        {
            rb.AddForce(new Vector3 (0, -1, 0) * movingIntensity, ForceMode.Force);

            if (transform.position.y <= -32)
            {
                rb.constraints = RigidbodyConstraints.FreezePosition; //ne zaboravi da unfreezuješ
                _should_enemy_sink = false;
                spawnInterval = Random.Range(1, 6);
                _enemy_sank = true;
            }
        }

        if(_enemy_sank && spawnTimer < spawnInterval)
        {
            spawnTimer += Time.fixedDeltaTime;
        }

        if(_enemy_sank && spawnTimer >= spawnInterval)
        {
            _enemy_sank = false;
            spawnTimer = 0;
            spawnInterval = Random.Range(1, 6);
            MoveToRandomLocationOutsidePlayerView();
        }

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

        //provera uslova za kretanje i stizanje u aproksimitet igracu

        if (distance > minDistance && !_is_on_distance && !_should_enemy_sink && !_enemy_sank)
        {
            rb.AddForce(movingDirectionNormalized * movingIntensity, ForceMode.Force);
        }
        else if(distance <= minDistance && (_is_on_distance || !_is_on_distance && !_enemy_sank))
        {
            rb.linearVelocity = Vector3.zero;
            _is_on_distance = true;
            //pokrece se tajmer za cannon
            _timer_currently_active = true;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "cannonball")
        {
            Sink(transform.position); //temporary 
            
            
            
        }
    }

    public void Sink(Vector3 ImpactWorldPosition) {

        //mozda treba da se obrise ovo sa vector3.zero; testirano na sudaru sa drugim brodom pa se zato brod odbijao onako ludacki
        rb.linearVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        explosion.transform.position = ImpactWorldPosition;
        explosion_particle.Play();
        boxCollider.enabled = false;
        RockUpAndDown.enabled = false;
        _should_enemy_sink = true;


    }
    public void SpawnSelf() {

        if ((transform.position.x >= -100 && transform.position.y <= 100) || (transform.position.z >= -100 && transform.position.z <= 100))
        {
            Debug.Log("nova pozicija je preblizu playeru");
            MoveToRandomLocationOutsidePlayerView();
        }

        else if (Physics.Raycast(transform.position, movingDirectionNormalized, out RaycastHit hit))
        {
            if (hit.collider.tag != "Player" && hit.collider.tag != "cannonball") //staviti tag koji je jednak "Enemy"
            {
                Debug.Log("raycast je detektovao objekat na putu do playera");
                MoveToRandomLocationOutsidePlayerView();
            }
        }

        else
        {
            Debug.Log("trebalo bi da brod krene");
            EnemyChildren.SetActive(true);
            _is_on_distance = false;
            boxCollider.enabled = true;
            _enemy_sank = false;
            RockUpAndDown.enabled = true;
            rb.constraints = RigidbodyConstraints.None;
            transform.LookAt(player);
        }

    }

    public void MoveToRandomLocationOutsidePlayerView()
    {
        EnemyChildren.SetActive(false);
        transform.position = new Vector3(Random.Range(-400, 400), 8, Random.Range(-400, 400));
        SpawnSelf();


    }




    

}


