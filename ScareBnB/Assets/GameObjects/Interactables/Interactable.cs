using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool interacting = false;
    
    public virtual void Interact()
    {
        interacting = !interacting;
    }
}
