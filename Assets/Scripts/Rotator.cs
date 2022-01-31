using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameover) return;

        rotationSpeed = GameManager.surviveTime;
        if (rotationSpeed > 60f)
        {
            rotationSpeed = 60f;
        }
        transform.Rotate(0f, rotationSpeed * Time.deltaTime / 2, 0f);
    }
}
