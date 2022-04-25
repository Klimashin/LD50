using TMPro;
using UnityEngine;

public class DayUI : MonoBehaviour
{
    [SerializeField] private CampSystem _campSystem;

    private TextMeshProUGUI _dayText;
    private void Awake()
    {
        _dayText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        _dayText.text = $"Day: {_campSystem.CurrentDay.ToString()}";
    }
}
