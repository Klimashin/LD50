using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class GeneratedItem : MonoBehaviour, ICharacterInteraction
{
    [Range(0, 100)]
    public int GenerationChance = 100;

    private void Awake()
    {
        GenerationCheck();
    }

    public void GenerationCheck()
    {
        var roll = Random.Range(0, 101);
        if (roll > GenerationChance)
        {
            Destroy(gameObject);
        }
    }
    
    private readonly Color _highlightOnColor = Color.red;
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
