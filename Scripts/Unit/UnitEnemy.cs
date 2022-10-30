using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEnemy : Unit
{
    [SerializeField] private GameObject enemyDestroyer;
    [SerializeField] private GameObject backgroundParticle;
    [SerializeField] private GameObject destroyParticle;
    [SerializeField] private float _screenres;

    void Update()
    {
        transform.RotateAround(transform.position, 0.6f * Time.deltaTime);
        
        if (gameObject.transform.position.y <= -10)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEvent(Collider2D collision)
    {
        if (collision.tag == TagEnum.Blade.ToString())
        {
            _scoreManager.AddScore();
            
            Vector3 direction = (collision.transform.position - transform.position).normalized;
            
            Instantiate(enemyDestroyer, transform.position, Quaternion.LookRotation(direction));
            
            InitBgParticle(collision);
            InitDestroyParticle(collision, direction);
            transform.position = new Vector2(
                    Screen.width * transform.position.x, Screen.height * transform.position.y) / _screenres
            ;
            
            Destroy(gameObject);
        }

        if (collision.tag == TagEnum.DamageArea.ToString())
        {
            Destroy(gameObject);
            _healthController.ApplyDamage();
        }
    }

    private void InitBgParticle(Collider2D collision)
    {
        var bgParticleInst = Instantiate(
            backgroundParticle, 
            transform.position + collision.transform.position, 
            Quaternion.identity
        ) as GameObject;
        bgParticleInst.transform.SetParent(_HUD.transform, false);
        Destroy(bgParticleInst, 1f);
    }

    private void InitDestroyParticle(Collider2D collision, Vector3 direction)
    {
        var destroyParticleInst = Instantiate(
            destroyParticle, transform.position + Vector3.forward * 4, Quaternion.LookRotation(direction)
        );
        Destroy(destroyParticleInst, 0.2f);
    }
}