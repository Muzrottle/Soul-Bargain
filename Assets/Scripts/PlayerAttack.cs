using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    bool canDamage = false;
    public bool CanDamage { get { return canDamage; } }
    bool didDamage = false;
    public bool DidDamage { get { return didDamage; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(canDamage);
    }

    public void DealedDamage()
    {
        canDamage = false;
        didDamage = true;
    }

    private void DamageStart()
    {
        canDamage = true;
        didDamage = false;
    }

    private void DamageEnd()
    {
        canDamage = false;
    }
}
