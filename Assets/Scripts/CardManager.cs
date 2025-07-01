using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    [System.Serializable]
    public class CardCategory
    {
        public string name;
        public GameObject cardBackPrefab;
        public List<string> cards = new List<string>();
        public CategoryButton buttonReference;
    }

    [SerializeField] private List<CardCategory> allCategories;
    private Dictionary<CardCategory, List<string>> availableCards = new Dictionary<CardCategory, List<string>>();
    private GameObject currentCard;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetDeck()
    {
        availableCards.Clear();
        foreach (var category in allCategories)
        {
            if (category.buttonReference.IsCategoryEnabled())
            {
                availableCards.Add(category, new List<string>(category.cards));
            }
        }
        
        if(currentCard != null) Destroy(currentCard);
    }

    public void DrawCard()
    {
        if (!TryGetRandomCategory(out CardCategory category))
        {
            GameManager.Instance.ShowEndGame();
            return;
        }

        if(currentCard != null) Destroy(currentCard);
        
        currentCard = Instantiate(category.cardBackPrefab);
        currentCard.GetComponent<CardController>().Initialize(GetRandomCard(category));
    }

    private bool TryGetRandomCategory(out CardCategory category)
    {
        var activeCategories = new List<CardCategory>(availableCards.Keys);
        activeCategories.RemoveAll(c => availableCards[c].Count == 0);
        
        if (activeCategories.Count == 0)
        {
            category = null;
            return false;
        }

        category = activeCategories[Random.Range(0, activeCategories.Count)];
        return true;
    }

    private string GetRandomCard(CardCategory category)
    {
        int randomIndex = Random.Range(0, availableCards[category].Count);
        string cardText = availableCards[category][randomIndex];
        availableCards[category].RemoveAt(randomIndex);
        return cardText;
    }
}
