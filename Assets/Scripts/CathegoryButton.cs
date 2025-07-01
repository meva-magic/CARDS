using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CategoryButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Image disabledOverlay;
    
    private bool isEnabled = true;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ToggleCategory);
    }

    public void Initialize(Sprite iconSprite, string categoryName)
    {
        icon.sprite = iconSprite;
        label.text = categoryName;
        UpdateVisuals();
    }

    private void ToggleCategory()
    {
        isEnabled = !isEnabled;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        disabledOverlay.gameObject.SetActive(!isEnabled);
    }

    public bool IsCategoryEnabled() => isEnabled;
}
