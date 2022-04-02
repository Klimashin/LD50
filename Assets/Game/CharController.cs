using UnityEngine;
using UnityEngine.InputSystem;

public class CharController : MonoBehaviour
{
    public float Speed;
    public Transform RendererTransform;

    [SerializeField] private CampSystem _campSystem;
    

    public void Enable()
    {
        Game.InputActions.Gameplay.Action.performed += OnAction;
    }

    private void OnAction(InputAction.CallbackContext obj)
    {
        // implement me
    }

    private void Update()
    {
        var input = Game.InputActions.Gameplay.Move.ReadValue<Vector2>();
        var direction = Vector2.zero;
        if (input.y > 0)
            direction += Vector2.up;
        
        if (input.y < 0)
            direction += Vector2.down;
        
        if (input.x < 0)
            direction += Vector2.left;
        
        if (input.x > 0)
            direction += Vector2.right;

        if (direction != Vector2.zero)
        {
            transform.Translate(direction * (Speed * Time.deltaTime));
            RendererTransform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Food>(out var food))
        {
            _campSystem.CurrentFood += food.Amount;
            Destroy(other.gameObject);
            // play sound
            // play particle vfx
        }
    }
}
