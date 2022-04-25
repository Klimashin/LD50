using TMPro;
using UnityEngine;

public class FoodUI : MonoBehaviour
{
    [SerializeField] private CampSystem _campSystem;

    private TextMeshProUGUI _foodText;
    private void Awake()
    {
        _foodText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        _foodText.text = $"X {_campSystem.CurrentFood.ToString()}";
    }
}
