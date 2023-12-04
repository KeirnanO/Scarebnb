using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

public class ActivityPoint : MonoBehaviour
{
    public bool available = false;
    public float activityLength  = 3f;
    public float pointMultiplyer = 1f;

    public void SetAvailable(bool availability)
    {
        available = availability;
    }

    public void StartActivity(Human human)
    {
        StartCoroutine(ActivityLoop(human));
    }

    IEnumerator ActivityLoop(Human human)
    {
        float endTime = Time.time + activityLength;

        while (Time.time < endTime)
        {
            yield return new WaitForSeconds(1 / pointMultiplyer);

            human.ReceivePoints(1);
        }

        available = true;
        human.FinishActivity();
    }

    public void EndActivity()
    {
        available = true;
        StopAllCoroutines();
    }
}
