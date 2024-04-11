using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] GameObject enemySword;
    public GameObject EnemySword { get { return enemySword; } }

    Animator enemyAnim;
    Transform player;
    CharacterController characterController;
    NavMeshAgent navMeshAgent;

    bool canLook = true;
    bool isAttacking = false;
    bool isMoving = false;
    bool canDamage = false;
    public bool CanDamage { get { return canDamage; } }
    bool isDamaged = false;
    bool isDied = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerMovement>().transform;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDied)
        {
            if (Vector3.Distance(gameObject.transform.position, player.position) > navMeshAgent.stoppingDistance)
            {
                ChasePlayer();
            }
            else
            {
                AttackPlayer();
            }
        }
    }

    private void ChasePlayer()
    {
        if (CanMove())
        {
            isMoving = true;
            isAttacking = false;
            enemyAnim.SetBool("isAttacking", isAttacking);
            enemyAnim.SetBool("isMoving", isMoving);
        }
        
        if (!isAttacking)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0f;
            gameObject.transform.DORotateQuaternion(Quaternion.LookRotation(directionToPlayer), 0.1f)
            .SetEase(Ease.Linear);
            navMeshAgent.SetDestination(player.position);
        }
        //characterController.Move(gameObject.transform.forward * movementSpeed * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        if (enemyAnim.GetBool("isMoving"))
        {
            isMoving = false;
            enemyAnim.SetBool("isMoving", isMoving);
        }

        if (CanAttack())
        {
            isAttacking = true;
            enemyAnim.SetBool("isAttacking", isAttacking);
        }

        if (canLook)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0f;
            gameObject.transform.DORotateQuaternion(Quaternion.LookRotation(directionToPlayer), 0.1f)
            .SetEase(Ease.Linear);
        }
    }

    public void AttackBlocked()
    {
        canDamage = false;
    }

    public void Damaged()
    {
        isAttacking = false;
        enemyAnim.Play("Idle", 1, 0f);
        enemyAnim.Play("Damaged", 2, 0f);
        //isDamaged = true;
    }

    public void Died()
    {
        enemyAnim.Play("Died");
        isDied = true;
        //isDamaged = true;
    }

    public void DamagedPlayer()
    {
        canDamage = false;
    }

    bool CanMove() => !isAttacking && !isMoving && !isDamaged;
    bool CanAttack() => !isAttacking && !isMoving && !isDamaged;

    private void AttackLook()
    {
        canLook = false; 
    }

    private void CanLookAgain()
    {
        canLook = true;
        canDamage = true;
    }
    private void SetAttackFalse()
    {
        isAttacking = false;
    }

    private void SetAttackTrue()
    {

    }
}
