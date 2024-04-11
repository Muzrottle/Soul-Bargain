using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    EnemyAI enemyAI;
    PlayerHealth playerHealth;
    PlayerAnimationHandler playerAnimationHandler;

    private void Start()
    {
        enemyAI = FindObjectOfType<EnemyAI>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<EnemyAI>() != null)
        {
            if (other.gameObject == other.GetComponentInParent<EnemyAI>().EnemySword && !playerAnimationHandler.IsDodging && !playerAnimationHandler.IsDead)
            {
                if (enemyAI.CanDamage)
                {
                    playerHealth.PlayerDamaged(10);
                    enemyAI.DamagedPlayer();
                }
            }
        }
    }
}
