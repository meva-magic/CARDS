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
        if (Input.acceleration.sqrMagnitude >= sqrShakeThreshold)
        {
            OnShake();
        }
    }

    private void OnShake()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameActive)
        {
            CardManager.Instance.DrawCard();
        }
    }

    private void OnDestroy()
    {
        spaceAction?.Dispose();
    }
}
