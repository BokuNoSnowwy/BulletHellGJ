using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionFollowingEnemy : Enemy
{
    [Header("Direction")]
    [SerializeField] 
    private Vector2 _dirToFollow;
    // Start is called before the first frame update
    void Start()
    {
         Initialization();
    }

    // Update is called once per frame
    void Update()
    {
        Tick();

        transform.Translate(_dirToFollow.normalized * movementSpeed * Time.deltaTime);
    }
}
