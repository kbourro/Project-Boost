﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    Vector3 movementVector;

    [SerializeField]
    float period = 2f;

    [Range(0, 1)]
    [SerializeField]
    float movementFactor;

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(period<=Mathf.Epsilon) { return; }
        float cycles = Time.time / period;
        float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles*tau);
        movementFactor = rawSinWave /2f +0.5f;
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
