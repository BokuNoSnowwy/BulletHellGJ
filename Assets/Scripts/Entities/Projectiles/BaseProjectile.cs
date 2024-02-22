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
    protected Vector3 baseScale;
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

        if (!CameraManager.IsVisibleFrom(_spriteRenderer, Camera.main))
        {
            ReleaseFromPool();
        }
    }
    
    public void OnTakenFromPool()
    {
        gameObject.SetActive(true);

        if (baseScale == Vector3.zero)
        {
            baseScale = transform.localScale;
        }
    }

    public void SetupProjectile(float dmg, float spd, float scale = 0, Sprite sprite = null)
    {
        _projectileSpd = spd;
        _projectileDmg = dmg;
        transform.localScale = baseScale * (1 + scale);
        
        if (sprite != null)
        {
            _spriteRenderer.sprite = sprite;
        }
        
        _projectileIsSetup = true;
    }

    protected virtual void ReleaseFromPool() { }

    protected virtual void OnTriggerEnter2D(Collider2D col) { }
}
