using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] bool stopJumping;
    public bool StopJumping { get { return stopJumping; }}
}
