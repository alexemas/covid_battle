using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private GameObject lifePoint;
    
    private int _healthPoint = 0;
    private GameObject[] _lifePoints;
    private Text _loseText;
    
    public delegate void OnDeath();
    public delegate void OnApplyDamage(int damage = 1);

    private void Start()
    {
        _healthPoint = maxHealth;
        
        _loseText = GameObject.Find("lose").GetComponent<Text>();
        _loseText.enabled = false;

        _lifePoints = GameObject.FindGameObjectsWithTag(TagEnum.HealthPoint.ToString());
    }

    private void FixedUpdate()
    {
        if (!IsAlive())
        {
            Death();
        }
    }

    public void ApplyDamage(int damage = 1)
    {
        SetHealthPoint(_healthPoint - damage);
        DisableLivePoint(_healthPoint);
    }

    public void Heal(int healPoint = 1)
    {
        SetHealthPoint(_healthPoint + healPoint);
    }
    
    public void Death()
    {
        SetHealthPoint(0);
        
        for (int i = 0; i < _lifePoints.Length; i++)
        {
            DisableLivePoint(i);
        }
        
        _loseText.text = "YOU LOSE";
        _loseText.enabled = true;
        Time.timeScale = 0;
    }

    private bool IsAlive()
    {
        return _healthPoint > 0;
    }

    private void SetHealthPoint(int newHealthPoint)
    {
        _healthPoint = Mathf.Clamp(newHealthPoint, 0, maxHealth);
    }

    private void DisableLivePoint(int index)
    {
        var currentPoint = maxHealth - (index + 1);

        if (currentPoint < 0) return;

        var nextPoint = maxHealth - index;
        _lifePoints[currentPoint].GetComponent<Animator>().SetBool("Destroyed", true);

        if (nextPoint < _lifePoints.Length - 1)
        {
            _lifePoints[nextPoint].GetComponent<Animator>().enabled = true;
        }
    }
}