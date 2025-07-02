using TMPro;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
public class CardController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject cardFront;
    [SerializeField] private GameObject cardBack;
    [SerializeField] private TextMeshProUGUI cardText;

    private bool isFlipped;
    private Sequence flipSequence;

    private void OnValidate()
    {
        if (cardFront == null) cardFront = transform.Find("Front")?.gameObject;
        if (cardBack == null) cardBack = transform.Find("Back")?.gameObject;
        if (cardText == null) cardText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Initialize(string text)
    {
        if (cardText != null) 
            cardText.text = text;
        
        ResetCard();
    }

    public void FlipCard()
    {
        if (flipSequence != null && flipSequence.IsActive())
            return;

        isFlipped = !isFlipped;
        
        flipSequence = DOTween.Sequence()
            .Append(transform.DORotate(new Vector3(0, 90, 0), 0.25f))
            .AppendCallback(() => ToggleCardFaces())
            .Append(transform.DORotate(Vector3.zero, 0.25f));
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

    private void OnDestroy()
    {
        flipSequence?.Kill();
    }
}
