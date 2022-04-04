
using UnityEngine;

public class Crate : GeneratedItem
{
    public int Amount;
    public Sprite BrokenSprite;
    public AudioClip Sfx;
    [SerializeField] private SoundSystem _soundSystem;
    
    public override void Execute(CharController character)
    {
        UI.controller.GetUIElement<GameplayUIScreen>().AddFoodAnimated(Amount);
        GetComponentInChildren<SpriteRenderer>().sprite = BrokenSprite;
        Highlight(false);
        Destroy(this);
        _soundSystem.PlayOneShot(Sfx);
    }
}
