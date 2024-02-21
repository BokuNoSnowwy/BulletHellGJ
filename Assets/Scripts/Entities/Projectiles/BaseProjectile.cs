using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] 
    protected SpriteRenderer _spriteRenderer;
    protected float _projectileDmg;
    protected float _projectileSpd;

    protected bool _projectileIsSetup = false;

    protected ObjectsPoolingManager _poolingManager;
    private void Start()
    {
        _poolingManager = ObjectsPoolingManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_projectileIsSetup)
        {
            transform.Translate(transform.right * Time.deltaTime * _projectileSpd, Space.World); 
        }

        if (!CameraFollow.IsVisibleFrom(_spriteRenderer, Camera.main))
        {
            ReleaseFromPool();
        }
    }
    
    public void OnTakenFromPool()
    {
        gameObject.SetActive(true);
        //Debug.Log("Enemy spawned");
    }

    public void SetupProjectile(float dmg, float spd, float scale = 1, Sprite sprite = null)
    {
        _projectileSpd = spd;
        _projectileDmg = dmg;
        transform.localScale *= scale;
        
        if (sprite != null)
        {
            _spriteRenderer.sprite = sprite;
        }
        
        _projectileIsSetup = true;
    }

    protected virtual void ReleaseFromPool() { }

    protected virtual void OnTriggerEnter2D(Collider2D col) { }
}
