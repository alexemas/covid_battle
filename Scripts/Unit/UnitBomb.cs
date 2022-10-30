using UnityEngine;

public class UnitBomb : Unit
{
    void Update()
    {
        transform.RotateAround(transform.position, 0.6f * Time.deltaTime);
    }
    
    protected override void OnTriggerEvent(Collider2D collision)
    {
        if (collision.tag == TagEnum.Blade.ToString())
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<HealthController>().Death();
        }

        if (collision.tag == TagEnum.DamageArea.ToString())
        {
            Destroy(gameObject);
        }
    }
}