using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    [SerializeField] float souls = 0;

    public void GainSouls(float amount)
    {
        souls += amount;
    }

    public void UseSouls(float amount)
    {
        souls -= amount;
    }
}
