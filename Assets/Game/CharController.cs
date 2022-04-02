using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharController : MonoBehaviour
{
    public float Speed;
    public Transform RendererTransform;

    [SerializeField] private CampSystem _campSystem;

    private InputBuffer _inputBuffer = new InputBuffer();

    public void Enable()
    {
        Game.InputActions.Gameplay.Action.performed += OnAction;
    }

    private void OnAction(InputAction.CallbackContext obj)
    {
        _inputBuffer.Action = true;
    }

    private void Update()
    {
        var input = Game.InputActions.Gameplay.Move.ReadValue<Vector2>();
        if (input.y != 0)
        {
            _inputBuffer.Up = input.y > 0;
            _inputBuffer.Down = input.y < 0;
        }

        if (input.x != 0)
        {
            _inputBuffer.Left = input.x < 0;
            _inputBuffer.Right = input.x > 0;
        }
    }

    private void FixedUpdate()
    {
        var direction = Vector2.zero;
        if (_inputBuffer.Up)
            direction += Vector2.up;
        
        if (_inputBuffer.Down)
            direction += Vector2.down;
        
        if (_inputBuffer.Left)
            direction += Vector2.left;
        
        if (_inputBuffer.Right)
            direction += Vector2.right;

        if (direction != Vector2.zero)
        {
            transform.Translate(direction * (Speed * Time.deltaTime));
            RendererTransform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
        
        _inputBuffer.Flush();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Food>(out var food))
        {
            _campSystem.CurrentFood += food.Amount;
            // play sound
            // play particle vfx
        }
    }

    private class InputBuffer
    {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;
        public bool Action;

        public void Flush()
        {
            Up = Down = Left = Right = Action = false;
        }
    }
}
