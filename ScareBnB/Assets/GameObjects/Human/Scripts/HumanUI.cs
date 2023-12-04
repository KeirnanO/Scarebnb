using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HumanUI : MonoBehaviour
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

    public Canvas canvas;
    public TextMeshProUGUI sanityText;
    public TextMeshProUGUI pointsText;

    private void Start()
    {
        Human.OnScare += OnScare;
        Human.OnReceivePoints += OnReceivePoints;
    }

    private void Update()
    {
        canvas.transform.LookAt(Camera.main.transform.position, Camera.main.transform.up);
    }

    void OnScare(int sanity)
    {
        sanityText.SetText(sanity.ToString());
    }

    void OnReceivePoints(int amount)
    {
        pointsText.enabled = true;

        pointsText.SetText(amount.ToString());
    }
}
