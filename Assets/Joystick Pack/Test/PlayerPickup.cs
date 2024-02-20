using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRadius = 2f; // Le rayon de d�tection des objets ramassables

    void Update()
    {
        // D�tection des objets ramassables
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);

        GameObject nearestPickup = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            // V�rifie si le collider appartient � un objet ramassable
            if (col.CompareTag("Pickup"))
            {
                float distanceToPickup = Vector3.Distance(transform.position, col.transform.position);
                if (distanceToPickup < nearestDistance)
                {
                    nearestPickup = col.gameObject;
                    nearestDistance = distanceToPickup;
                }
            }
        }

        // Ramasser l'objet le plus proche
        if (nearestPickup != null)
        {
            PickupItem(nearestPickup);
        }
    }

    void PickupItem(GameObject item)
    {
        // Effectuer l'action de ramassage appropri�e
        // Par exemple, vous pouvez ajouter de l'XP au joueur ou restaurer sa sant�
        Debug.Log("je suis du " + item.name);


        Destroy(item);
    }
}
