using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Entity
{
    public static Player Instance;

    public GameObject deathMenu;

    //Exp an Level
    [SerializeField] AnimationCurve ExpCapCurve;
    public int currentExp;
    public int maxExp = 25;
    public int currentLevel = 0 ;

    //Life
    [Header("Life")]
   // public int maxHealth = 100;
   // public int currentHealth;
    public HealthBar healthBar;
    
    [Header("XP Panel")] 
    [SerializeField] 
    private GameObject _levelUpPanel;
    [SerializeField] 
    private PlayerInventory _playerInventory;
    
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
        if (!_playerInventory)
        {
            _playerInventory = GetComponent<PlayerInventory>();
        }
        healthBar.SetMaxHealth((int)_maxLife);
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
            LevelingUp();
        }
    }
    private void ChangeLevelCap()
    {
        maxExp = (int)ExpCapCurve.Evaluate(currentLevel);
    }

    [ContextMenu("LevelingUp")]
    private void LevelingUp()
    {
        if (!_playerInventory.IsItemGameEmpty())
        {
            _gameManager.SetGameState(GameState.Pause);
            _playerInventory.DisplayUpgrades(()=> _levelUpPanel.SetActive(false));
            _levelUpPanel.SetActive(true);
        }
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        Debug.Log("Ouch");
        healthBar.SetHealth((int)_life);
    }

    public void AddMaxLife(int maxLife)
    {
        _maxLife += maxLife;
        _life += maxLife;
    }

    protected override void Die()
    {
        //deathMenu.SetActive(true);

        _gameManager.SetGameState(GameState.Pause);
        //Time.timeScale = 0f;

        //TODO Disable player controller
        //TODO Death animation 
    }
}
