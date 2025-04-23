using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Cannon CannonObject;
    public float DistanceFromCenter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

  

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Sink(Vector3 ImpactWorldPosition) { }
    public void SpawnSelf() { }

    public void MoveToRandomLocationOutsidePlayerView() { }
}
