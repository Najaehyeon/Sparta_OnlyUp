using UnityEngine;

public class InteractMessage : MonoBehaviour
{
    public Transform fromCamera;

    private void Update()
    {
        transform.forward = transform.position - fromCamera.transform.position;
    }
}
