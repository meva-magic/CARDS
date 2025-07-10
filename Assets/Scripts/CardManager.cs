using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-90)]
public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    [System.Serializable]
    public class CardCategory
    {
        public string name;
        public GameObject categoryBackPrefab;
        public GameObject[] cardPrefabs;
    }

    [Header("Settings")]
    [SerializeField] private List<CardCategory> categories;
    [SerializeField] private Transform cardSpawnPoint;
    [SerializeField] private int drawVibrationDuration = 50;
    
    private Dictionary<CardCategory, Queue<GameObject>> availableCards;
    private GameObject currentCardObject;
    private CardCategory currentCategory;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializeDeck();
    }

    private void InitializeDeck()
    {
        availableCards = new Dictionary<CardCategory, Queue<GameObject>>();
        foreach (var category in categories)
        {
            var queue = new Queue<GameObject>(category.cardPrefabs);
            availableCards.Add(category, queue);
        }
    }

    public void SelectCategory()
    {
        ReleaseCurrentCard();

        foreach (var category in categories)
        {
            if (availableCards[category].Count > 0)
            {
                currentCategory = category;
                currentCardObject = Instantiate(category.categoryBackPrefab, cardSpawnPoint);
                
                Vibration.VibratePeek();
                Shake.instance.CamShake();
                return;
            }
        }
    }

    public void DrawCard()
    {
        if (currentCategory == null || availableCards[currentCategory].Count == 0)
        {
            SelectCategory();
            return;
        }

        ReleaseCurrentCard();

        int randomIndex = Random.Range(0, availableCards[currentCategory].Count);
        currentCardObject = Instantiate(
            availableCards[currentCategory].Dequeue(),
            cardSpawnPoint.position,
            cardSpawnPoint.rotation
        );
        
        Vibration.Vibrate(drawVibrationDuration);
        Shake.instance.CamShake();
    }

    private void ReleaseCurrentCard()
    {
        if (currentCardObject != null)
        {
            Destroy(currentCardObject);
        }
    }

    public void ResetDeck()
    {
        ReleaseCurrentCard();
        currentCategory = null;
        InitializeDeck();
    }
}
