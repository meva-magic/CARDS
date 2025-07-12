using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class CardController : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive())
            return;

        if (Shake.instance != null)
        {
            Shake.instance.ScreenShake();
        }

        if (CardManager.Instance != null)
        {
            CardManager.Instance.HandleCardPress();
        }
    }
}
