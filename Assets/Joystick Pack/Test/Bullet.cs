using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float timeToDestroy = 8f;
    private float speed = 10f;
    private float timespawn = 0f;
    
    void Start()
    {
        /*Rigidbody rb = this.GetComponent<Rigidbody>();


        if (rb != null)
        {
            rb.velocity = this.transform.forward * speed;
        }
        else
        {
            Debug.LogWarning("Projectile prefab does not have a Rigidbody component!");
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timespawn >= timeToDestroy)
        {
            Destroy(this.gameObject);
        }
    }
}
