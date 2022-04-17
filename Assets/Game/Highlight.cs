using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : MonoBehaviour
{
    public GameObject _highlightOn;
    public Image _characterImage;
    public CampSystem _campSystem;

    private Dictionary<string, string> _charactersNameDict = new Dictionary<string, string>()
    {
        { "DadImage", "Dad" },
        { "MomImage", "Mom" },
        { "KidImage", "Kid" }
    };

    public void HandleMouseEnter()
    {
        string characterName = _charactersNameDict[_characterImage.name];
        if (
            _characterImage.IsActive()
            && _campSystem.Characters[characterName].IsAlive
            && !_campSystem.Characters[characterName].IsFed
            && _campSystem.CurrentFood >= _campSystem.Characters[characterName].FoodRequired
        )
        {
            _highlightOn.SetActive(true);
        }
    }

    private void Update()
    {
        string characterName = _charactersNameDict[_characterImage.name];
        if (!_campSystem.Characters[characterName].IsAlive
            || _campSystem.Characters[characterName].IsFed
            || _campSystem.CurrentFood < _campSystem.Characters[characterName].FoodRequired)
        {
            _highlightOn.SetActive(false);
        }
    }
}
