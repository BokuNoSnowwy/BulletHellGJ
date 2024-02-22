using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRadius = 2f; // Le rayon de détection des objets ramassables

    public Player player;
   

    void Update()
    {
        // Détection des objets ramassables
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);

        GameObject nearestPickup = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            // Vérifie si le collider appartient à un objet ramassable
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
        // Effectuer l'action de ramassage appropriée
        // Par exemple, vous pouvez ajouter de l'XP au joueur ou restaurer sa santé
        Debug.Log("je suis du " + item.name);

        

        Destroy(item);
    }
}
