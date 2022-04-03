
public class Crate : GeneratedItem
{
    public int Amount;
    
    public override void Execute(CharController character)
    {
        UI.controller.GetUIElement<GameplayUIScreen>().AddFoodAnimated(Amount);
        Destroy(gameObject);
    }
}
