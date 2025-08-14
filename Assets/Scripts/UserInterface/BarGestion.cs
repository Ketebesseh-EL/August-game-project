using UnityEngine;
using UnityEngine.UI;

public class BarGestion : MonoBehaviour
{
    public Slider slider;

    public void SetMax(float max)
    {
        slider.maxValue = max;
        slider.value = max;
    }


    public void SetValue(float value)
    {
        slider.value = value;
    }
}
