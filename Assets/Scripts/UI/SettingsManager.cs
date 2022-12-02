using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;

    public void OnSensitivitySettingChange() {
        GameManager.playerSettings.sensitivity = slider.value;
        sliderText.text = string.Format("{0:0.0}", slider.value);
    }
}
