using UnityEngine;
using UnityEngine.SceneManagement;

public class BottledMessage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GetComponent<Rigidbody>().linearVelocity = -transform.position.normalized*6f;
        GetComponent<AudioSource>().PlayDelayed(1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) { SceneManager.LoadScene("EndScene"); }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
