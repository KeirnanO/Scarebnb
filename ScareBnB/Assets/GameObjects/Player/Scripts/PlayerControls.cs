using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Events;
using System;
using Unity.VisualScripting;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerControls : MonoBehaviour
{

    private Controls controls;
    public Controls Controls
    {
        get
        {
            if (controls == null) 
            {
                controls = new Controls(); 
            }
            return controls;
        }
    }

    private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement
    {
        get { 
            if (playerMovement == null)
                playerMovement = GetComponent<PlayerMovement>();
            return playerMovement; 
        }
    }

    public Animator animator;
    
    //Interaction
    private Interactable currentInteractable = null;
    public bool interacting = false;
    public float interactionRadius = 3.0f;
    public LayerMask interactionLayerMask;

    private void OnEnable() => Controls.Enable();
    private void OnDisable() => Controls.Disable();

    private void Start()
    {
        Controls.Player.Move.performed += ctx => PlayerMovement.SetMovement(ctx.ReadValue<Vector2>());
        Controls.Player.Move.canceled += ctx => PlayerMovement.SetMovement(Vector2.zero);

        Controls.Player.Interact.performed += ctx => Interact();
    }

    private void Update()
    {
        if (interacting)
        {
            return;
        }

        if(PlayerMovement)
            playerMovement.Move();
    }
    private void Interact()
    {
        if (interacting)
        {
            EndInteract();
            return;
        }

        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, interactionRadius, Vector3.one, 1000.0f, interactionLayerMask);

        if (hits.Length == 0)
        {
            print("No hits");
            return;
        }

        int closestHitIndex = 0;
        if (hits.Length > 1)
        {
            float shortestDistance = interactionRadius;
            for (int i = 0; i < hits.Length; i++)
            {
                float hitDistance = (hits[i].transform.position - transform.position).magnitude;
                if (hitDistance < shortestDistance)
                {
                    shortestDistance = hitDistance;
                    closestHitIndex = i;
                }                
            }
        }        

        interacting = true;
        currentInteractable = hits[closestHitIndex].transform.GetComponent<Interactable>();
        currentInteractable.Interact();

        transform.position = currentInteractable.transform.position;
        playerMovement.StopMovement();
        animator.SetBool("Interacting", true);
    }

    public void EndInteract()
    {
        interacting = false;
        currentInteractable.Interact();
        animator.SetBool("Interacting", false);

        currentInteractable = null;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        if (interacting)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
    }
}
