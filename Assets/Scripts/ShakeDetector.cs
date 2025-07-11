using UnityEngine;
using UnityEngine.InputSystem;

public class ShakeDetector : MonoBehaviour
{
    [SerializeField] private float shakeThreshold = 2f;
    private float sqrShakeThreshold;
    private InputAction spaceAction;

    private void Start()
    {
        if (Shake.instance == null)
        {
            Debug.LogError("Shake instance not found!");
            enabled = false;
            return;
        }

        sqrShakeThreshold = Mathf.Pow(shakeThreshold, 2);
        
        spaceAction = new InputAction(binding: "<Keyboard>/space");
        spaceAction.Enable();
        spaceAction.performed += OnShakePerformed;
    }

    private void Update()
    {
        if (GameManager.Instance != null && 
            GameManager.Instance.IsGameActive() && 
            Input.acceleration.sqrMagnitude >= sqrShakeThreshold)
        {
            OnShake();
        }
    }

    private void OnShakePerformed(InputAction.CallbackContext context)
    {
        OnShake();
    }

    private void OnShake()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive() || CardManager.Instance == null)
            return;

        CardManager.Instance.DrawCard();
        Vibration.VibratePeek();
        Shake.instance.ScreenShake();
    }

    private void OnDestroy()
    {
        if (spaceAction != null)
        {
            spaceAction.performed -= OnShakePerformed;
            spaceAction.Dispose();
        }
    }
}
