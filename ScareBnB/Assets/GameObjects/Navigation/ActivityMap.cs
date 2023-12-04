using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityMap : MonoBehaviour
{
    public static ActivityMap instance;

    public ActivityPoint[] activityPoints = null;
    public Transform homePoint = null;

    private void Awake()
    {
        instance = this;
    }
    
    public ActivityPoint GetActivityPoint()
    {
        foreach (var point in activityPoints)
        {
            if(point.available)
            {
                return point;
            }
        }

        return null;
    }

    public ActivityPoint GetRandomActivityPoint()
    {
       int random = Random.Range(0, activityPoints.Length);

        ActivityPoint point = activityPoints[random];

        if (point.available)
        {
            return point;
        }

        return null;
    }

}
