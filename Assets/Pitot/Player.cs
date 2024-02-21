using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Entity
{
    public static Player Instance;

    public int exp;
    public int level;
    private float lastAttackTime;
    
    // Multiplier
    public float attackSpeedMultiplier = 1f;
    public float movementSpeedMultiplier = 1f;

    private GameManager _gameManager;

    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }
    
    
    void Start()
    {
        _gameManager = GameManager.Instance;
    }
    void Update()
    {

    }

    public void GainExp(int amount)
    {
        exp += amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (exp >= 10)
        {
            level++;
            exp = 0;
            Debug.Log("Level Up! Current Level: " + level);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        _gameManager.SetGameState(GameState.Pause);
        //TODO Disable player controller
        //TODO Death animation 
    }
}
