using UnityEngine;
using UnityEngine.UI;

public class CategoryToggle : MonoBehaviour
{
    [SerializeField] private int categoryIndex;
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.SetIsOnWithoutNotify(true);
            toggle.onValueChanged.AddListener(OnToggleChanged);
            Debug.Log($"Category toggle {categoryIndex} initialized");
        }
        else
        {
            Debug.LogError("Toggle component missing!", this);
        }
    }

    private void OnToggleChanged(bool isOn)
    {
        if (CardManager.Instance == null)
        {
            Debug.LogWarning("CardManager not ready, queuing toggle update");
            StartCoroutine(UpdateWhenReady(isOn));
            return;
        }
        UpdateCategory(isOn);
    }

    private System.Collections.IEnumerator UpdateWhenReady(bool isOn)
    {
        yield return new WaitUntil(() => CardManager.Instance != null);
        UpdateCategory(isOn);
    }

    private void UpdateCategory(bool isOn)
    {
        CardManager.Instance.ToggleCategory(categoryIndex, isOn);
    }

    private void OnDestroy()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }
    }
}
