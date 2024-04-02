using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] bool isInteractable;
    public bool IsInteractable { get { return isInteractable; }}
    [SerializeField] bool isClosable;
    public bool IsClosable { get { return isClosable; } }
    [SerializeField] bool isInteracted;
    public bool IsInteracted { get { return isInteracted; } }
    [SerializeField] bool isChest;
    [SerializeField] bool isSword;

    [SerializeField] GameObject item;
    [SerializeField] List<AnimationClip> animations;

    InteractHandler interactHandler;
    Animation objectAnim;
    ParticleSystem chestItemParticle;
    PlayerMovement playerMovement;

    bool isItemActive;

    private void Start()
    {
        objectAnim = GetComponent<Animation>();
        interactHandler = FindObjectOfType<InteractHandler>();
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (isChest)
        {
            chestItemParticle = gameObject.GetComponentInChildren<ParticleSystem>();
            chestItemParticle.gameObject.SetActive(false);
        }
    }

    public void Interacted()
    {
        if (!isInteracted)
        {
            if (isSword)
            {
                DropCurrentSword();
                Destroy(gameObject.GetComponent<Rigidbody>());
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.GetComponent<Obstacle>().isInteractable = false;
                gameObject.transform.SetParent(playerMovement.PlayersWeaponHand);
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                return;
            }

            if (isChest && chestItemParticle.gameObject.activeInHierarchy)
            {
                PickUpItem();
                return;
            }

            isInteracted = true;
            objectAnim.Play(animations[0].name);

            if (!isClosable)
            {
                interactHandler.InteractTextSetActive(false);
            }

            if (isChest)
            {
                StartCoroutine(ChestOpened());
            }
        }
        else if (isInteracted && isClosable)
        {
            isInteracted = false;
            objectAnim.Play(animations[1].name);
            Debug.Log("Oldu");
        }
    }

    private void PickUpItem()
    {
        isInteracted = true;
        chestItemParticle.gameObject.SetActive(false);
        interactHandler.InteractTextSetActive(false);
        DropCurrentSword();
        Instantiate(item, playerMovement.PlayersWeaponHand);
    }

    private static void DropCurrentSword()
    {
        Transform currentWeapon = FindObjectOfType<PlayerMovement>().GetComponentInChildren<Weapon>().gameObject.transform;
        currentWeapon.SetParent(null);
        currentWeapon.transform.position = FindObjectOfType<PlayerMovement>().transform.position + FindObjectOfType<PlayerMovement>().transform.right + FindObjectOfType<PlayerMovement>().transform.up;
        currentWeapon.GetComponent<BoxCollider>().isTrigger = false;
        currentWeapon.GetComponent<Obstacle>().isInteractable = true;
        currentWeapon.AddComponent<Rigidbody>();
    }

    IEnumerator ChestOpened()
    {
        yield return new WaitForSeconds(1f);
        chestItemParticle.gameObject.SetActive(true);
        isInteracted = false;
    }
}
