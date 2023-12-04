using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearInteractable : Interactable
{
    [SerializeField] private LayerMask HumanLayerMask;
    
    public float scareTime = 0.1f;
    public float scareRadius = 2f;

    public int scareMultiplyer = 1;

    public override void Interact()
    {
        //Interact with object
        base.Interact();


        //If we are no longer interacting, stop all interaction processes
        if(!interacting)
        {
            //Stop animations
            return;
        }

        //StartAnimations
        StartCoroutine(FearInduceLoop());

    }

    IEnumerator FearInduceLoop()
    {
        while (interacting)
        {
            yield return new WaitForSeconds(scareTime);

            RaycastHit[] hits;
            hits = Physics.SphereCastAll(transform.position, scareRadius, Vector3.one, 100f, HumanLayerMask);

            foreach(RaycastHit hit in hits)
            {
                Human human = hit.transform.GetComponent<Human>();
                human.RemoveSanity(scareMultiplyer);
            }            
        }
    }

    private void OnDrawGizmos()
    {
        if (interacting)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, scareRadius);
        }
    }
}
