using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    [System.Serializable]
    public class CardCategory
    {
        public string name;
        public bool isEnabled = true;
        public GameObject cardBackPrefab;
        public List<GameObject> cardFacePrefabs;
    }

    [Header("References")]
    [SerializeField] private Transform cardSpawnPoint;
    [SerializeField] private List<CardCategory> categories = new();

    private GameObject currentCard;
    private CardCategory selectedCategory;
    private bool isCardRevealed = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetDeck()
    {
        ClearCurrentCard();
        selectedCategory = null;
        isCardRevealed = false;
    }

    public void DrawCard()
    {
        if (!GameManager.Instance.IsGameActive() || cardSpawnPoint == null) return;

        ClearCurrentCard();
        isCardRevealed = false;

        var availableCategories = categories.FindAll(c => 
            c.isEnabled && c.cardFacePrefabs != null && c.cardFacePrefabs.Count > 0);

        if (availableCategories.Count == 0)
        {
            GameManager.Instance.EndGame();
            return;
        }

        selectedCategory = availableCategories[Random.Range(0, availableCategories.Count)];
        
        if (selectedCategory.cardBackPrefab != null)
        {
            currentCard = Instantiate(
                selectedCategory.cardBackPrefab,
                cardSpawnPoint.position,
                cardSpawnPoint.rotation,
                cardSpawnPoint
            );

            // Add controller to back card
            var controller = currentCard.AddComponent<CardController>();
            
            // Add collider if missing
            if (currentCard.GetComponent<BoxCollider2D>() == null)
            {
                var collider = currentCard.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(100f, 150f);
            }
        }
    }

    public void RevealCard()
    {
        if (isCardRevealed || currentCard == null || selectedCategory == null || 
            selectedCategory.cardFacePrefabs == null || 
            selectedCategory.cardFacePrefabs.Count == 0)
            return;

        Vector3 position = cardSpawnPoint.position;
        Quaternion rotation = cardSpawnPoint.rotation;
        Transform parent = cardSpawnPoint;

        Destroy(currentCard);

        int randomIndex = Random.Range(0, selectedCategory.cardFacePrefabs.Count);
        currentCard = Instantiate(
            selectedCategory.cardFacePrefabs[randomIndex],
            position,
            rotation,
            parent
        );

        // Add controller to face card
        var controller = currentCard.AddComponent<CardController>();
        
        // Add collider if missing
        if (currentCard.GetComponent<BoxCollider2D>() == null)
        {
            var collider = currentCard.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(100f, 150f);
        }

        selectedCategory.cardFacePrefabs.RemoveAt(randomIndex);
        isCardRevealed = true;
    }

    public void HandleCardPress()
    {
        if (!isCardRevealed)
        {
            RevealCard();
        }
        
        // Screen shake is handled by CardController
    }

    public void ToggleCategory(int categoryIndex, bool isOn)
    {
        if (categoryIndex >= 0 && categoryIndex < categories.Count)
        {
            categories[categoryIndex].isEnabled = isOn;
        }
    }

    public bool HasEnabledCategories()
    {
        foreach (var category in categories)
        {
            if (category.isEnabled)
                return true;
        }
        return false;
    }

    public bool HasCardsRemaining()
    {
        foreach (var category in categories)
        {
            if (category.isEnabled && category.cardFacePrefabs.Count > 0)
                return true;
        }
        return false;
    }

    private void ClearCurrentCard()
    {
        if (currentCard != null)
        {
            Destroy(currentCard);
            currentCard = null;
        }
    }
}
