using UnityEngine;

public class DragVisual : MonoBehaviour
{
    public Vector3 FirstPoint, SecondPoint;
    public bool IsEnabled;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnabled)
        {

            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, FirstPoint);
                lineRenderer.SetPosition(1, SecondPoint);
            }
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (lineRenderer != null)
            {
                lineRenderer.enabled = false;
            }
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }
}