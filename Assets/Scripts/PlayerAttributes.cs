using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField] Slider staminaSlider;
    [SerializeField] Image staminaFill;

    [SerializeField] float maxStamina = 50;
    [SerializeField] float staminaRegenSpeed = 5f;
    [SerializeField] float staminaDrainSpeed = 3f;
    [SerializeField] float staminaRegenStartTime = 2f;

    float currentStamina;
    float timePastSinceStaminaDrained;

    PlayerAnimationHandler playerAnimationHandler;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (timePastSinceStaminaDrained < staminaRegenStartTime)
        {
            timePastSinceStaminaDrained += Time.deltaTime;
        }

        if (timePastSinceStaminaDrained >= staminaRegenStartTime)
        {
            RegenarateStamina();
        }

        if (playerAnimationHandler.IsSprinting)
        {
            DrainStamina();
        }
    }

    private void RegenarateStamina()
    {
        currentStamina += staminaRegenSpeed * Time.deltaTime;
        staminaSlider.value = currentStamina;
    }

    private void DrainStamina()
    {
        currentStamina -= staminaDrainSpeed * Time.deltaTime;
        staminaSlider.value = currentStamina;
        timePastSinceStaminaDrained = 0;
    }

    public void DrainInstantStamina(float drainAmount)
    {
        currentStamina -= drainAmount;
        staminaSlider.value = currentStamina;
        timePastSinceStaminaDrained = 0;
    }
}
