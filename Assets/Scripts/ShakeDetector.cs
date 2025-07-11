using UnityEngine;
using UnityEngine.InputSystem;

public class ShakeDetector : MonoBehaviour
{
    [SerializeField] private float shakeThreshold = 2f;
    private float sqrShakeThreshold;
    private InputAction spaceAction;

    private void Start()
    {
        sqrShakeThreshold = Mathf.Pow(shakeThreshold, 2);
        
        spaceAction = new InputAction(binding: "<Keyboard>/space");
        spaceAction.Enable();
        spaceAction.performed += _ => OnShake();
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

    private void OnShake()
    {
        if (GameManager.Instance != null && 
            GameManager.Instance.IsGameActive() && 
            CardManager.Instance != null)
        {
            CardManager.Instance.DrawCard();
            Vibration.VibratePeek();
            Shake.instance.ScreenShake();
        }
    }

    private void OnDestroy()
    {
        if (spaceAction != null)
        {
            spaceAction.performed -= _ => OnShake();
            spaceAction.Dispose();
        }
    }
}
