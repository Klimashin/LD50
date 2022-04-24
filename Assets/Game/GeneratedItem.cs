using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GeneratedItem : MonoBehaviour, ICharacterInteraction, IWorldObjectRandomDependent
{
    [Range(0, 100)]
    public int GenerationChance = 100;

    public void Initialize(int roll)
    {
        if (roll > GenerationChance)
        {
            Destroy(gameObject);
        }
    }

    private readonly Color _highlightOnColor = Color.green;
    private readonly Color _highlightOffColor = Color.white;
    private List<SpriteRenderer> _renderers;
    public void Highlight(bool isOn)
    {
        _renderers ??= GetComponentsInChildren<SpriteRenderer>().ToList();

        foreach (var r in _renderers)
        {
            r.color = isOn ? _highlightOnColor : _highlightOffColor;
        }
    }
    
    public virtual void Execute(CharController character)
    { }
}
