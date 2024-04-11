using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float weaponDmg = 10f;
    PlayerAttack playerAttack;
    PlayerAnimationHandler playerAnimationHandler;

    private void Start()
    {
        playerAttack = GetComponentInParent<PlayerAttack>();
        playerAnimationHandler = GetComponentInParent<PlayerAnimationHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() && playerAnimationHandler.IsAttacking)
        {
            if (!playerAttack.DidDamage && playerAttack.CanDamage)
            {
                other.gameObject.GetComponent<Enemy>().GotDamaged(weaponDmg);
                playerAttack.DealedDamage();
            }
        }

        if (other.GetComponentInParent<EnemyAI>() != null)
        {
            if (other.GetComponentInParent<EnemyAI>().CanDamage)
            {
                if (other.gameObject == other.GetComponentInParent<EnemyAI>().EnemySword && playerAnimationHandler.IsBlocking)
                {
                    playerAnimationHandler.Blocked();
                }
            }
        }
    }
}
