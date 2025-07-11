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

    public void InitializeDeck()
    {
        ClearCurrentCard();
        selectedCategory = null;
    }

    public void DrawCard()
    {
        if (!GameManager.Instance.IsGameActive() || cardSpawnPoint == null) return;

        ClearCurrentCard();

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
            
            // Ensure the card back can be clicked
            var controller = currentCard.GetComponent<CardController>();
            if (controller == null)
            {
                controller = currentCard.AddComponent<CardController>();
            }
        }
    }

    public void RevealCard()
    {
        if (currentCard == null || selectedCategory == null || 
            selectedCategory.cardFacePrefabs == null || 
            selectedCategory.cardFacePrefabs.Count == 0) return;

        Vector3 spawnPos = cardSpawnPoint.position;
        Quaternion spawnRot = cardSpawnPoint.rotation;
        Transform parent = cardSpawnPoint;

        Destroy(currentCard);

        int randomIndex = Random.Range(0, selectedCategory.cardFacePrefabs.Count);
        currentCard = Instantiate(
            selectedCategory.cardFacePrefabs[randomIndex],
            spawnPos,
            spawnRot,
            parent
        );
        selectedCategory.cardFacePrefabs.RemoveAt(randomIndex);
    }

    public void ToggleCategory(int categoryIndex, bool isOn)
    {
        if (categoryIndex >= 0 && categoryIndex < categories.Count)
        {
            categories[categoryIndex].isEnabled = isOn;
        }
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
