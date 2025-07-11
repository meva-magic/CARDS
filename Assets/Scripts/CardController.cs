using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class CardController : MonoBehaviour, IPointerDownHandler
{
    private bool isRevealed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive())
            return;

        if (CardManager.Instance != null && Shake.instance != null)
        {
            if (!isRevealed)
            {
                CardManager.Instance.RevealCard();
                isRevealed = true;
            }
            else
            {
                Shake.instance.ScreenShake();
            }
        }
    }

    public void ResetCard()
    {
        isRevealed = false;
    }
}
