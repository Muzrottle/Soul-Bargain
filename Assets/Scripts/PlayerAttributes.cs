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
    public float CurrentStamina { get { return currentStamina; } }
    float timePastSinceStaminaDrained;

    PlayerAnimationHandler playerAnimationHandler;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (timePastSinceStaminaDrained < staminaRegenStartTime)
        {
            timePastSinceStaminaDrained += Time.deltaTime;
        }

        if (timePastSinceStaminaDrained >= staminaRegenStartTime && currentStamina != maxStamina)
        {
            RegenarateStamina();
        }
    }

    private void RegenarateStamina()
    {
        currentStamina += staminaRegenSpeed * Time.deltaTime;
        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }
        staminaSlider.value = currentStamina;
    }

    public void DrainStamina()
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

    public bool CheckEnoughStamina(float staminaCost)
    {
        if (currentStamina >= staminaCost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
