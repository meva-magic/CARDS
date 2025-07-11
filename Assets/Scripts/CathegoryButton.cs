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
        // Get the required Toggle component
        _toggle = GetComponent<Toggle>();
        
        if (_toggle == null)
        {
            Debug.LogError("[CategoryToggle] Toggle component is missing on " + gameObject.name, this);
            enabled = false;
            return;
        }

        // Find overlay if not assigned
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

        // Initialize overlay state
        if (overlay != null)
        {
            overlay.SetActive(false);
        }
        else
        {
            Debug.LogWarning("[CategoryToggle] Overlay reference not found on " + gameObject.name, this);
        }

        // Remove any existing listeners to prevent duplicates
        _toggle.onValueChanged.RemoveAllListeners();
        
        // Add new listener
        _toggle.onValueChanged.AddListener(HandleToggleChange);
        
        // Set initial state without notification
        _toggle.SetIsOnWithoutNotify(true);
        
        Debug.Log("[CategoryToggle] Initialized on " + gameObject.name);
    }

    private void HandleToggleChange(bool isOn)
    {
        if (_isProcessing) return;
        _isProcessing = true;
        
        StartCoroutine(ProcessToggleChange(isOn));
    }

    private IEnumerator ProcessToggleChange(bool isOn)
    {
        Debug.Log($"[CategoryToggle] Toggle changed to {isOn} on {gameObject.name}");
        
        // First handle the visual feedback
        if (overlay != null)
        {
            overlay.SetActive(!isOn);
            Debug.Log($"[CategoryToggle] Overlay set to {!isOn}");
        }

        // Then handle the CardManager update
        if (CardManager.Instance == null)
        {
            Debug.LogWarning("[CategoryToggle] CardManager not ready, waiting...");
            yield return new WaitUntil(() => CardManager.Instance != null);
        }

        try
        {
            CardManager.Instance?.ToggleCategory(categoryIndex, isOn);
            Debug.Log($"[CategoryToggle] CardManager updated for category {categoryIndex}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[CategoryToggle] Error updating CardManager: {e.Message}");
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
        // Auto-assign the toggle reference in editor
        if (_toggle == null)
        {
            _toggle = GetComponent<Toggle>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
    #endif
}
