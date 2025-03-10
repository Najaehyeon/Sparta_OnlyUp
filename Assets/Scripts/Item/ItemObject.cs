using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData itemData;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && itemData.itemType == ItemType.Jump)
        {
            CharacterManager.Instance.Player.jumpPower *= itemData.stat;
            Destroy(gameObject);
        }
    }
}
