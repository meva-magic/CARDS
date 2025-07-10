using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class CategoryButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Image disabledOverlay;

    private Button button;
    private bool isEnabled = true;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonPressed);
        UpdateVisuals();
    }

    private void OnButtonPressed()
    {
        Vibration.VibratePeek();
        Shake.instance.CamShake();
        ToggleCategory();
    }

    private void ToggleCategory()
    {
        isEnabled = !isEnabled;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (disabledOverlay != null)
            disabledOverlay.gameObject.SetActive(!isEnabled);
    }

    public bool IsEnabled => isEnabled;
}
