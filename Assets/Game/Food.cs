using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Food : GeneratedItem, ICharacterInteraction
{
    public int Amount;
    public AudioClip Sfx;
    [SerializeField] private SoundSystem _soundSystem;

    public override void Execute(CharController character)
    {
        UI.controller.GetUIElement<GameplayUIScreen>().AddFoodAnimated(Amount);
        Destroy(gameObject);
        _soundSystem.PlayOneShot(Sfx);
        
        WorldObjectRef.AddHistoryEvent(new HistoryDisableEvent( ID ));
    }
}
