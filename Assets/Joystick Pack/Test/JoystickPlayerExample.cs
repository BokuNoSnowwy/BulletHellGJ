using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    /*public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;*/

    /*public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }*/

    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        
        if (direction != Vector3.zero)
        {
            // Calculer la rotation pour que le joueur regarde dans la direction du mouvement
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Appliquer la rotation au joueur, en gardant sa rotation en Y (pour éviter de basculer)
            transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
        }
    }
}