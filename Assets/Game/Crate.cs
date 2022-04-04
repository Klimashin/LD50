
using UnityEngine;

public class Crate : GeneratedItem
{
    public int Amount;
    public Sprite BrokenSprite;
    
    public override void Execute(CharController character)
    {
        UI.controller.GetUIElement<GameplayUIScreen>().AddFoodAnimated(Amount);
        GetComponentInChildren<SpriteRenderer>().sprite = BrokenSprite;
        Highlight(false);
        Destroy(this);
    }
}
