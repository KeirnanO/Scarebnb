using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Human)), RequireComponent(typeof(Animator))]
public class HumanAnimationController : MonoBehaviour
{
    private Human human;
    public Human Human
    {
        get
        {
            if (human == null)
                human = GetComponent<Human>();
            return human;
        }
    }

    private Animator animator;
    public Animator Animator
    {
        get
        {
            if (animator == null)
                animator = GetComponent<Animator>();
            return Animator;
        }
    }

    private void Start()
    {
        Human.OnScare += AnimOnScare;
    }

    void AnimOnScare(int sanity)
    {
        if(sanity <= 0)
            Animator.SetBool("Scared", true);
    }
}
