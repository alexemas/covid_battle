using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllBonus : Unit
{
    private float _pointForEvent = 1;
    
    protected override void OnTriggerEvent(Collider2D collision)
    {
        if (collision.tag == TagEnum.Blade.ToString())
        {
            Vector3 direction = (collision.transform.position - transform.position).normalized;

            _spawnerController.DestroyUnits();

            Destroy(gameObject);        
        }
    }
}
