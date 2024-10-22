using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] Transform lookAt;

    void Update()
    {
        transform.LookAt(lookAt);
    }
}
