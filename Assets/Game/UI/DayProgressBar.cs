
using UnityEngine.UI;

public class DayProgressBar : UIWidget
{
    public Image ProgressBar;

    public void UpdateProgressBar(float normalizedValue)
    {
        ProgressBar.fillAmount = normalizedValue;
    }
}
