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

            opponent.OpponentHealth -= opponent.HealthDecreaseAmount;
            opponent.OpponentHealth = Mathf.Max(
                opponent.OpponentHealth, 0
            );
        }
        
        if(!collision.collider.CompareTag("Player"))
        { Destroy(gameObject); }
    }
}
