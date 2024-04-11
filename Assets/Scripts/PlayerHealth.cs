using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Image healthFill;

    [SerializeField] float maxHealth = 50;

    float currentHealth;

    PlayerAnimationHandler playerAnimationHandler;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerDamaged(float damageAmount)
    {
        currentHealth -= damageAmount;
        healthSlider.value = currentHealth;
        playerAnimationHandler.DamagedAnim();

        if (currentHealth <= 0)
        {
            playerAnimationHandler.DeathAnim();
        }
    }
}
