
public class Crate : GeneratedItem
{
    public int Amount;
    
    public override void Execute(CharController character)
    {
        character.AddFood(Amount);
        Destroy(gameObject);
    }
}
