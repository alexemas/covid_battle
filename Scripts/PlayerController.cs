using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject sprite;
    [SerializeField] private float velocity = .00005f;
    [SerializeField] private float time;
    [SerializeField] private float starterTime;
    private BladeController _eventManager;

    public float Velocity => velocity;
    public GameObject Sprite => sprite;
    public float Time => time;
    public float StarterTime => starterTime;

    void Start()
    {
        _eventManager = GetComponent<BladeController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _eventManager.StartCutting();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _eventManager.StopCutting();
        }

        _eventManager.Cut();
    }
}