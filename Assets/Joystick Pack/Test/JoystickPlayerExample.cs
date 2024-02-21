using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;

    public Transform rotationShooting;
    public void FixedUpdate()
    {
        //Vector3 direction = Vector3.up * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

        //transform.Translate(direction * speed * Time.deltaTime);

        
        float moveHorizontal = variableJoystick.Horizontal;
        float moveVertical = variableJoystick.Vertical;

        
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f) * speed;

        
        rb.velocity = movement;

        
        if (movement.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(moveVertical, moveHorizontal) * Mathf.Rad2Deg;
            rotationShooting.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }
    }

    
}