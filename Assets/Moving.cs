using Unity.VisualScripting;
using UnityEngine;

public class Moving : MonoBehaviour
{
    public Transform player;
    public float minDistance = 100f;
    public Rigidbody rb;
    float distance;
    Vector3 movingDirection;
    float movingIntensity = 25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movingDirection = (player.position - transform.position).normalized;
        transform.LookAt(player);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector3.Distance(player.position, transform.position);

        if(distance > minDistance)
        {
            rb.AddForce(movingDirection * movingIntensity, ForceMode.Force);
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
}
