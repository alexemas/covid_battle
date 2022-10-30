using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    protected Canvas _HUD;
    protected HealthController _healthController;
    protected UnitSpawnerController _spawnerController;
    protected ScoreManager _scoreManager;
    
    private void Start()
    {
        var playerController = GameObject.Find("PlayerController");
        
        _healthController = playerController.GetComponent<HealthController>();
        _spawnerController = Camera.main.GetComponent<UnitSpawnerController>();
        _scoreManager = playerController.GetComponent<ScoreManager>();
        _HUD = GameObject.FindWithTag(TagEnum.HUD.ToString()).GetComponent<Canvas>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
       OnTriggerEvent(collision);
    }

    protected abstract void OnTriggerEvent(Collider2D collision);
}