using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class CardController : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive())
            return;

        if (CardManager.Instance != null && Shake.instance != null)
        {
            CardManager.Instance.RevealCard();
            Vibration.VibratePop();
            Shake.instance.ScreenShake();
        }
    }
}
