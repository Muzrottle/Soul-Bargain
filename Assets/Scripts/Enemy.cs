using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float enemyMaxHealth = 100f;

    float currentEnemyHealth;

    LookAtMouse lookAtMouse;
    EnemyDetection enemyDetection;

    // Start is called before the first frame update
    void Start()
    {
        currentEnemyHealth = enemyMaxHealth;
        lookAtMouse = FindObjectOfType<LookAtMouse>();
        enemyDetection = FindObjectOfType<EnemyDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GotDamaged(float damageAmount)
    {
        currentEnemyHealth -= damageAmount;
        Debug.Log("AH");

        if (currentEnemyHealth == 0)
        {
            lookAtMouse.RemoveFocus();
            enemyDetection.RemoveEnemy(gameObject.transform);
            Destroy(gameObject);
        }
    }
}
