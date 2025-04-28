using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Cannon CannonObject;
    public float DistanceFromCenter;

    public AudioClip BoatMovingSound;
    public AudioSource AudioSource;

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
    bool _is_first_spawn = true;
    bool _sound_stopped = false;
    bool _enemy_hit_by_ray = false;




    private void Start()
    {
        interval = Random.Range(5, 10);
        spawnTimer = 0;
        rb.linearVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        boxCollider.enabled = false;
        RockUpAndDown.enabled = false;
        _should_enemy_sink = true;
        AudioSource.enabled = false;
        AudioSource.volume = 0.3f;
    }

    void FixedUpdate()
    {
        distance = Vector3.Distance(player.position, transform.position);
        movingDirection = (player.position - transform.position);
        movingDirectionNormalized = movingDirection.normalized;

        if (_should_enemy_sink && !_enemy_sank)
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

        if(_enemy_sank && spawnTimer < spawnInterval && !_is_first_spawn)
        {
            spawnTimer += Time.fixedDeltaTime;
        }

        if(_enemy_sank && spawnTimer >= spawnInterval && !_is_first_spawn)
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
            //stopira se zvuk po pravilu

            if (AudioSource.isPlaying && !_sound_stopped)
            {
                AudioSource.Stop();
                _sound_stopped = true;
            }
            rb.constraints = RigidbodyConstraints.None;
            //pokrece se tajmer za cannon
            _timer_currently_active = true;
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

        movingDirection = (player.position - transform.position);
        movingDirectionNormalized = movingDirection.normalized;

        RaycastHit[] hit = Physics.SphereCastAll(transform.position, 10, movingDirectionNormalized, Vector3.Distance(player.position, transform.position));




        if ((transform.position.x >= -100 && transform.position.x <= 100) || (transform.position.z >= -100 && transform.position.z <= 100))
        {
            MoveToRandomLocationOutsidePlayerView();
        }

        else if (hit != null && hit.Length > 0)
        {
            foreach (RaycastHit h in hit) {
                if (h.collider.tag == "Enemy" && h.collider.gameObject != this.gameObject)
                {
                    _enemy_hit_by_ray = true;
                    MoveToRandomLocationOutsidePlayerView();
                    break;
                }
                _enemy_hit_by_ray = false;
            }

            EnemyChildren.SetActive(true);
            _is_on_distance = false;
            boxCollider.enabled = true;
            _enemy_sank = false;
            _should_enemy_sink = false;
            RockUpAndDown.enabled = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            transform.LookAt(player);

            if (!_enemy_hit_by_ray) {
                _sound_stopped = false;
                AudioSource.enabled = true;
                AudioSource.loop = true;
                AudioSource.Play();
            }
        }

        else
        {
            EnemyChildren.SetActive(true);
            _is_on_distance = false;
            boxCollider.enabled = true;
            _enemy_sank = false;
            _should_enemy_sink = false;
            RockUpAndDown.enabled = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            transform.LookAt(player);

            //playuje se zvuk
            _sound_stopped = false;

            AudioSource.enabled = true;
            AudioSource.loop = true;
            AudioSource.Play();
        }

    }

    public void MoveToRandomLocationOutsidePlayerView()
    {
        EnemyChildren.SetActive(false);
        int sign1 = Random.Range(1, 3);
        int sign2 = Random.Range(1, 3);
        sign1 = (sign1 == 1) ? 1 : -1;
        sign2 = (sign2 == 1) ? 1 : -1;
        transform.position = new Vector3(Random.Range(200, 400) * sign1, 8, Random.Range(200, 400) * sign2);
        StartCoroutine(SpawnSelfCo());
        _is_first_spawn = false;


    }

    public IEnumerator SpawnSelfCo()
    {
        transform.LookAt(player);
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(1);
        SpawnSelf();
    }




    

}


