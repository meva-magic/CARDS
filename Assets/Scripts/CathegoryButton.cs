using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Toggle))]
public class CategoryToggle : MonoBehaviour
{
    [SerializeField] private int categoryIndex;
    [SerializeField] private GameObject overlay;
    
    private Toggle _toggle;
    private bool _isProcessing = false;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        
        if (_toggle == null)
        {
            Debug.LogError("[CategoryToggle] Toggle component missing");
            enabled = false;
            return;
        }

        if (overlay == null)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.name.Contains("Overlay"))
                {
                    overlay = child.gameObject;
                    break;
                }
            }
        }

        if (overlay != null)
        {
            overlay.SetActive(false);
        }

        _toggle.onValueChanged.RemoveAllListeners();
        _toggle.onValueChanged.AddListener(HandleToggleChange);
        _toggle.SetIsOnWithoutNotify(true);
    }

    private void HandleToggleChange(bool isOn)
    {
        if (_isProcessing) return;
        _isProcessing = true;
        StartCoroutine(ProcessToggleChange(isOn));
    }

    private IEnumerator ProcessToggleChange(bool isOn)
    {
        if (overlay != null)
        {
            overlay.SetActive(!isOn);
        }

        if (CardManager.Instance == null)
        {
            yield return new WaitUntil(() => CardManager.Instance != null);
        }

        try
        {
            CardManager.Instance?.ToggleCategory(categoryIndex, isOn);
        }
        finally
        {
            _isProcessing = false;
        }
    }

    private void OnDestroy()
    {
        if (_toggle != null)
        {
            _toggle.onValueChanged.RemoveListener(HandleToggleChange);
        }
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (_toggle == null)
        {
            _toggle = GetComponent<Toggle>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
    #endif
}
