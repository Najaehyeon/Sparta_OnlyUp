using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : InteractableObject
{
    public ItemData itemData;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = CharacterManager.Instance.Player.GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && itemData.itemType == ItemType.Jump)
        {
            playerController.jumpPower *= itemData.stat;
            Destroy(gameObject);
        }
    }
}
