using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;

    Animator enemyAnim;
    Transform player;
    CharacterController characterController;
    NavMeshAgent navMeshAgent;

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
        if (Vector3.Distance(gameObject.transform.position, player.position) > 1.5f)
        {
            if (!enemyAnim.GetBool("isMoving"))
            {
                enemyAnim.SetBool("isMoving", true);
            }
            //Vector3 directionToPlayer = player.position - transform.position;
            //directionToPlayer.y = 0f;
            //gameObject.transform.DORotateQuaternion(Quaternion.LookRotation(directionToPlayer), 0.1f)
            //.SetEase(Ease.Linear);
            navMeshAgent.SetDestination(player.position);
            //characterController.Move(gameObject.transform.forward * movementSpeed * Time.deltaTime);

        }
        else
        {
            enemyAnim.SetBool("isMoving", false);
        }
    }
}
