using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpPoint : MonoBehaviour
{
    private PlayerController controller;
    private Rigidbody playerRigidBody;

    private void Awake()
    {
        controller = CharacterManager.Instance.Player.GetComponent<PlayerController>();
        playerRigidBody = controller.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRigidBody.AddForce(Vector3.up * controller.jumpPower * 2f, ForceMode.Impulse);
        }
    }
}
