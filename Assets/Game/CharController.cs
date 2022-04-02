using UnityEngine;
using UnityEngine.InputSystem;

public class CharController : MonoBehaviour
{
    public float Speed;
    public Transform RendererTransform;
    public ParticleSystem FootstepParticles;
    public float FootstepDistance = 1f;
    public float FootstepGap = 0.25f;

    [SerializeField] private CampSystem _campSystem;

    private float _distanceTraveled;


    public void Enable()
    {
        Game.InputActions.Gameplay.Action.performed += OnAction;
    }

    private void OnAction(InputAction.CallbackContext obj)
    {
        // implement me
    }
    
    private int dir = 1;
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
            var distance = direction * (Speed * Time.deltaTime);
            _distanceTraveled += distance.magnitude;
            transform.Translate(distance);
            RendererTransform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

            Footstep();
        }
    }

    private void Footstep()
    {
        if (_distanceTraveled >= FootstepDistance)
        {
            _distanceTraveled = 0;
            var pos = transform.position + (RendererTransform.right * (FootstepGap * dir));
            dir *= -1;
            var ep = new ParticleSystem.EmitParams
            {
                position = pos, rotation = -RendererTransform.rotation.eulerAngles.z
            };
            FootstepParticles.Emit(ep, 1);
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
