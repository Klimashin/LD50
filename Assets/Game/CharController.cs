using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharController : MonoBehaviour
{
    public float Speed;
    public Transform RendererTransform;
    public ParticleSystem FootstepParticles;
    public Animator CharacterAnimator;
    public float FootstepDistance = 1f;
    public float FootstepGap = 0.25f;
    
    public Collider2D InteractionCollider;
    public ContactFilter2D InteractionColliderFilter;

    [SerializeField] private CampSystem _campSystem;

    private float _distanceTraveled;
    private UIPopup _foodPopup;

    public void Enable()
    {
        Game.InputActions.Gameplay.Action.performed += OnAction;
    }

    private void OnAction(InputAction.CallbackContext obj)
    {
        if (_highlightedInteraction != null)
        {
            _highlightedInteraction.Execute(this);
        }
    }
    
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private void Update()
    {
        InteractionHighlight();
        
        Movement();
    }

    private ICharacterInteraction _highlightedInteraction;
    private readonly List<Collider2D> _collidersCache = new List<Collider2D>();
    private void InteractionHighlight()
    {
        _collidersCache.Clear();
        var count = InteractionCollider.OverlapCollider(InteractionColliderFilter, _collidersCache);
        if (count == 0)
        {
            _highlightedInteraction?.Highlight(false);
            _highlightedInteraction = null;
            return;
        }

        ICharacterInteraction closestInteraction = null;
        var closestDistance = float.MaxValue;
        foreach (var col in _collidersCache)
        {
            var containsInteraction = col.TryGetComponent<ICharacterInteraction>(out var interaction);
            if (!containsInteraction)
                continue;

            var distance = Vector2.Distance(InteractionCollider.bounds.center, col.bounds.center);
            if (distance < closestDistance)
            {
                closestInteraction = interaction;
            }
        }
        
        if (closestInteraction == null)
        {
            _highlightedInteraction?.Highlight(false);
            _highlightedInteraction = null;
            return;
        }
        
        if (closestInteraction != _highlightedInteraction)
        {
            _highlightedInteraction?.Highlight(false);
            _highlightedInteraction = closestInteraction;
            _highlightedInteraction.Highlight(true);
        }
    }

    private void Movement()
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
        
        CharacterAnimator.SetBool(IsMoving, input.magnitude > 0);
    }

    private int _stepsDir = 1;
    private void Footstep()
    {
        if (_distanceTraveled >= FootstepDistance)
        {
            _distanceTraveled = 0;
            var pos = transform.position + (RendererTransform.right * (FootstepGap * _stepsDir));
            _stepsDir *= -1;
            var ep = new ParticleSystem.EmitParams
            {
                position = pos, rotation = -RendererTransform.rotation.eulerAngles.z
            };
            FootstepParticles.Emit(ep, 1);
        }
    }
    
    public void AddFood(int amount)
    {
        _campSystem.CurrentFood += amount;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ICharacterInteraction>(out var interaction))
        {
            interaction.Execute(this);
        }
    }
}
