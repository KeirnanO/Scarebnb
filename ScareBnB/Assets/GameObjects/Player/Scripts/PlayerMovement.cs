using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{  
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    [SerializeField] private float MoveSpeed = 4.0f;
    [Tooltip("Acceleration and deceleration")]
    [SerializeField] private float SpeedChangeRate = 10.0f;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody rb = null;

    private Vector2 previousInput = Vector2.zero;

    //Movement
    private float _speed = 4.0f;

    public void SetMovement(Vector2 movement)
    {
        previousInput = movement;
    }

    public void Move()
    {
        if (previousInput == Vector2.zero)
        {
            if (rb.velocity.magnitude > 0.1f)
                rb.velocity /= 1 + (10.0f * Time.deltaTime);
            else 
                rb.velocity = Vector2.zero;
            return;
        }

        float currentHorizontalSpeed = rb.velocity.magnitude;

        float speedOffset = 0.1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < MoveSpeed - speedOffset || currentHorizontalSpeed > MoveSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, MoveSpeed, Time.deltaTime * SpeedChangeRate);
        }
        else
        {
            _speed = MoveSpeed;
        }

        // normalise input direction
        Vector3 inputDirection = new Vector3(-previousInput.x, previousInput.y, 0f).normalized;

        // move the player
        rb.velocity = _speed * inputDirection;
    }

    public void StopMovement()
    {
        rb.velocity = Vector3.zero;
    }
}
