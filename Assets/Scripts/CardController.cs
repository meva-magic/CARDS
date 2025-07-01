using TMPro;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private GameObject cardBack;
    [SerializeField] private GameObject cardFront;
    [SerializeField] private TextMeshPro cardText;

    private bool isFlipped = false;

    public void Initialize(string text)
    {
        cardText.text = text;
        ResetCard();
    }

    public void FlipCard()
    {
        isFlipped = !isFlipped;
        cardBack.SetActive(!isFlipped);
        cardFront.SetActive(isFlipped);
    }

    private void ResetCard()
    {
        isFlipped = false;
        cardBack.SetActive(true);
        cardFront.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!isFlipped)
        {
            FlipCard();
        }
    }
}
