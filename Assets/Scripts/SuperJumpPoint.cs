using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpPoint : MonoBehaviour
{
    public Player player;
    private Rigidbody playerRigidBody;

    private void Awake()
    {
        playerRigidBody = player.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRigidBody.AddForce(Vector3.up * player.jumpPower * 2f, ForceMode.Impulse);
        }
    }
}
