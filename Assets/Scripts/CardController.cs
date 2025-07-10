using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CardController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject cardFront;
    [SerializeField] private GameObject cardBack;
    [SerializeField] private TextMeshProUGUI cardText;

    private bool isFlipped;

    public void Initialize(string text)
    {
        if (cardText != null) cardText.text = text;
        ResetCard();
    }

    public void FlipCard()
    {
        isFlipped = !isFlipped;
        
        Vibration.VibratePop();
        Shake.instance.CamShake();
        
        // Immediate flip without animation
        ToggleCardFaces();
    }

    private void ToggleCardFaces()
    {
        if (cardFront != null) cardFront.SetActive(isFlipped);
        if (cardBack != null) cardBack.SetActive(!isFlipped);
    }

    private void ResetCard()
    {
        isFlipped = false;
        ToggleCardFaces();
        transform.rotation = Quaternion.identity;
    }

    private void OnMouseDown()
    {
        if (!isFlipped)
            FlipCard();
    }
}
