using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] List<Transform> enemies = new List<Transform>();
    public List<Transform> Enemies { get { return enemies; } }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            enemies.Add(other.gameObject.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (enemies.Contains(other.gameObject.transform))
        {
            enemies.Remove(other.gameObject.transform);
        }
    }

    public void RemoveEnemy(Transform deadEnemy)
    {
        if (enemies.Contains(deadEnemy.gameObject.transform))
        {
            enemies.Remove(deadEnemy.gameObject.transform);
        }
    }
}
