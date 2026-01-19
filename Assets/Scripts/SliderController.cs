using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public int maxProgress = 3;
    public int minProgress = 1;

    public Slider slider;

    private int progress;

    private void Start()
    {
        progress = maxProgress;

        slider.minValue = minProgress;
        slider.maxValue = maxProgress;
        slider.value = progress;
    }

    public void DecreaseProgress()
    {
        progress--;

        progress = Mathf.Clamp(progress, minProgress, maxProgress);
        slider.value = progress;
    }

    public bool IsEmpty()
{
    return progress <= minProgress;
}

}
