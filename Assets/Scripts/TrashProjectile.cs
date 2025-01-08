using System.Collections;
using UnityEditor.UI;
using UnityEngine;

public class TrashProjectile : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Opponent"))
        {
            OpponentAI opponent =
                collision.collider.GetComponent<OpponentAI>(); 
            opponent.TakeDamage();
        }
        if(!collision.collider.CompareTag("Player"))
        { Destroy(gameObject); }
    }
}
