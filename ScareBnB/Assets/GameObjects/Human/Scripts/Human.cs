using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : MonoBehaviour
{
    enum CharacterState
    {
        Idle = 0,
        MoveToActivity = 1,
        DoingActivity = 2,
        Scared = 3
    }

    [SerializeField] private CharacterState mCharacterState;
    [SerializeField] private NavMeshAgent mNavMeshAgent;

    private ActivityPoint currentActivity;
    public int sanity = 100;
    public int points = 0;

    public Action<int> OnScare;
    public Action<int> OnReceivePoints;

    private void Update()
    {
        if (!mNavMeshAgent)
        {
            return;
        }

        switch(mCharacterState)
        {
            case CharacterState.Idle:
                if (currentActivity != null)
                    mCharacterState = CharacterState.MoveToActivity;

                FindNewActivity();

                break;

            case CharacterState.MoveToActivity:
                if(currentActivity == null)
                {
                    mCharacterState = CharacterState.Idle; 
                    break;
                }

                if (!mNavMeshAgent.pathPending)
                {
                    if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
                    {
                        if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f)
                        {
                            // Done
                            currentActivity.StartActivity(this);
                            mCharacterState = CharacterState.DoingActivity;
                        }
                    }
                }
                break;

            case CharacterState.DoingActivity:
                break;

            case CharacterState.Scared:
                if (!mNavMeshAgent.pathPending)
                {
                    if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
                    {
                        if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f)
                        {
                            // Done
                            Destroy(gameObject);
                        }
                    }
                }
                break;
        }

    }

    private void FindNewActivity()
    {
        ActivityPoint newPoint = ActivityMap.instance.GetRandomActivityPoint();

        if (newPoint == null)
        {
            return;
        }

        SetActivityPoint(newPoint);
    }

    private void Scare()
    {
        if(!mNavMeshAgent)
        {
            Destroy(gameObject); 
            return;
        }

        if(currentActivity)
        {
            currentActivity.EndActivity();
        }

        currentActivity = null;
        mCharacterState = CharacterState.Scared;

        //Return Home
        mNavMeshAgent.SetDestination(Vector3.zero);
    }

    public void SetActivityPoint(ActivityPoint point)
    {
        if(currentActivity != null)
        {
            currentActivity.EndActivity();
        }

        currentActivity = point;
        currentActivity.SetAvailable(false);

        mNavMeshAgent.SetDestination(currentActivity.transform.position);
    }
   
    public void RemoveSanity(int amount)
    {
        if (sanity < 0)
            return;

        sanity -= amount;
        OnScare?.Invoke(sanity);

        if(sanity <= 0)
        {
            Scare();
        }

    }

    public void ReceivePoints(int amount)
    {
        points += amount;

        OnReceivePoints?.Invoke(points);
    }

    public void FinishActivity()
    {
        currentActivity = null;
        mCharacterState = CharacterState.Idle;
    }
}
