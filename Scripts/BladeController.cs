using System;
using UnityEngine;

public class BladeController : MonoBehaviour
{
    private GameObject _sprite;
    private float _minVelocity;

    private Vector2 _previousPosition;
    private Camera _camera;
    private Transform _transform;
    private GameObject _currentTrail;
    private Rigidbody2D _rigidbody;
    private CircleCollider2D _collider;

    private void Start()
    {
        var controller = GetComponent<PlayerController>();
        
        _camera = Camera.main;
        _sprite = controller.Sprite;
        _minVelocity = controller.Velocity;
        _transform = controller.GetComponent<Transform>();
        _rigidbody = controller.GetComponent<Rigidbody2D>();
        _collider = controller.GetComponent<CircleCollider2D>();
    }

    public void Cut()
    {
        Vector2 newPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        _rigidbody.position = newPosition;

        float velocity = (newPosition - _previousPosition).magnitude * Time.deltaTime;
        
        if (Input.GetMouseButtonDown(0))
            _collider.enabled = velocity < _minVelocity;

        _previousPosition = newPosition;
    }

    public void StartCutting()
    {
        _currentTrail = Instantiate(_sprite, _transform);
        _previousPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        _collider.enabled = true;
    }

    public void StopCutting()
    {
        _currentTrail.transform.SetParent(null);
        Destroy(_currentTrail, 1f);
        _collider.enabled = false;
    }
}