using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private static Slider slider;
    [SerializeField] private static TextMeshProUGUI sliderText;

    public void OnSensitivitySettingChange() {
        GameManager.playerSettings.sensitivity = slider.value;
        sliderText.text = string.Format("{0:0.0}", slider.value);
    }
}
