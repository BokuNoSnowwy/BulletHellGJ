using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePoint : MonoBehaviour,IExperience
{
    public int ExpAmount = 5;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetExpAmount()
    {
        return ExpAmount;
    }
    public void OnTakenFromPool()
    {
        //_player = Player.Instance;
        gameObject.SetActive(true);
        //Debug.Log("Enemy spawned");
    }
}
