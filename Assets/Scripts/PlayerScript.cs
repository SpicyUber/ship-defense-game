using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Cannon CannonObject;
    public DragVisual Visual;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeExplosion(Vector3 ImpactWorldPosition) { }
    public float CooldownPercentage() { return 50; } //privremeno 50 za testiranje HUD-a.


}
