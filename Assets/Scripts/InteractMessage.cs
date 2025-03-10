using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMessage : MonoBehaviour
{
    public Transform camera;

    private void Update()
    {
        transform.forward = transform.position - camera.transform.position;
    }
}
