using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingDownBonus : Unit
{
    [SerializeField] 
    private float _time = 2;

    private float _seconds = 1;
    private MeshRenderer _mesh;

    bool _isCuted = false;

    void Awake()
    {
        _mesh = gameObject.GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        TimerStart();
        Time.fixedDeltaTime = Time.unscaledDeltaTime * Time.timeScale;
    }

    protected override void OnTriggerEvent(Collider2D collision)
    {
        if (collision.tag == TagEnum.Blade.ToString())
        {
            Vector3 direction = (collision.transform.position - transform.position).normalized;

            _isCuted = true;
            _mesh.enabled = false;  
            
            Instantiate(
                Resources.Load("Prefabs/Unit/UnitEnemyDestroyPrefab") as GameObject,
                transform.position,
                Quaternion.LookRotation(direction)
            );
            gameObject.GetComponent<Collider2D>().isTrigger = false;
        }
    }

    private void TimerStart()
    {
        if (_isCuted)
        {
            _time -= Time.deltaTime / _seconds;
            Time.timeScale = .5f;

            if (_time <= .1)
            {
                Time.timeScale = 1.0f;
            }
        }
    }
}
