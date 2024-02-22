using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Entity
{
    public static Player Instance;

   
    private float lastAttackTime;
    //Exp an Level
    [SerializeField] AnimationCurve ExpCapCurve;
    public int currentExp;
    public int maxExp = 25;
    public int currentLevel = 0 ;

    //Life
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

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
        healthBar.SetMaxHealth(maxHealth);
    }
    void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IExperience Exp = collision.GetComponent<IExperience>();
        if(Exp != null)
        {
            GainExp(Exp.GetExpAmount());
            Destroy(collision.gameObject); 
        }
    }
    public void GainExp(int Amount)
    {
        currentExp += Amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (currentExp >= maxExp)
        {
            currentLevel++;
            currentExp = currentExp-maxExp;
            ChangeLevelCap();
            Debug.Log("Level Up! Current Level: " + currentLevel);
        }
    }
    private void ChangeLevelCap()
    {
        maxExp = (int)ExpCapCurve.Evaluate(currentLevel);
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.SetHealth(currentHealth);
    }

    public void AddMaxLife(int maxLife)
    {
        _maxLife += maxLife;
        _life += maxLife;
    }

    protected override void Die()
    {
        _gameManager.SetGameState(GameState.Pause);
        //TODO Disable player controller
        //TODO Death animation 
    }
}
