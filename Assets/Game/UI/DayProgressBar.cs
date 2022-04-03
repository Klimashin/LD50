
using UnityEngine.UI;

public class DayProgressBar : UIWidget
{
    public Image ProgressBar;

    public void SetProgress(float normalizedValue)
    {
        ProgressBar.fillAmount = normalizedValue;
    }
}
