using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotation : MonoBehaviour
{

    //ale ta klasa jest chujowa xDDDD
    public float rotationAnglePerSecond = 15f;

    public bool startRotating = false;

    public bool resetRotation = true;

    void Update()
    {
        if (startRotating) transform.Rotate(0f, rotationAnglePerSecond, 0f * Time.deltaTime);
        if (!startRotating) transform.rotation = transform.rotation;
    }
}


