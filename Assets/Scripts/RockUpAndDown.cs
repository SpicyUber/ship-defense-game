using System;
using UnityEngine;

public class RockUpAndDown : MonoBehaviour
{
    public Transform player;
    public Rigidbody rb;

    Quaternion startRotation;
    bool _front_rotation_finished = false;
    bool _back_rotation_finished = true;
    bool pause = false;
    private float rotationSpeed = 2f;

    float timer = 0f;
    public float interval = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startRotation = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        if(pause)
        {
            timer += Time.deltaTime;

            if(timer >= interval)
            {
                timer = 0f;
                pause = false;
            }
        }

        if (!_front_rotation_finished && _back_rotation_finished && !pause)
        {
            Quaternion targetRotation = Quaternion.Euler(10f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) <= 1f)
            {
                _front_rotation_finished = true;
                pause = true;
                _back_rotation_finished = false;
            }
        }

        else if (_front_rotation_finished && !_back_rotation_finished && !pause)
        {
            Quaternion targetRotation = Quaternion.Euler(-10f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) <= 1f)
            {
                _back_rotation_finished = true;
                pause = true;
                _front_rotation_finished = false;
            }

        }

      

    }
}
