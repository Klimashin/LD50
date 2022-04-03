using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Food : GeneratedItem, ICharacterInteraction
{
    public int Amount;

    public override void Execute(CharController character)
    {
        character.AddFood(Amount);
        Destroy(gameObject);
    }
}
