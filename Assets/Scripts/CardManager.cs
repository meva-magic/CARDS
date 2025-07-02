using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

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
        public Button categoryButton;
        public GameObject selectionEffectPrefab;
        [HideInInspector] public ObjectPool<GameObject> cardPool;
    }

    [Header("Settings")]
    [SerializeField] private Transform cardSpawnPoint;
    [SerializeField] private GameObject cardRevealEffectPrefab;
    [SerializeField] private List<CardCategory> categories;

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
        InitializePools();
    }

    private void InitializePools()
    {
        availableCards = new Dictionary<CardCategory, Queue<GameObject>>();

        foreach (var category in categories)
        {
            var queue = new Queue<GameObject>(category.cardPrefabs);
            availableCards.Add(category, queue);

            category.cardPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(category.categoryBackPrefab),
                actionOnGet: (obj) => obj.SetActive(true),
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj)
            );
        }
    }

    public void SelectCategory()
    {
        ReleaseCurrentCard();

        foreach (var category in categories)
        {
            var button = category.categoryButton.GetComponent<CategoryButton>();
            if (button != null && button.IsEnabled && availableCards[category].Count > 0)
            {
                currentCategory = category;
                SpawnWithEffect(category.selectionEffectPrefab);
                currentCardObject = category.cardPool.Get();
                currentCardObject.transform.SetPositionAndRotation(
                    cardSpawnPoint.position, 
                    cardSpawnPoint.rotation
                );
                return;
            }
        }
        
        GameManager.Instance.ShowEndGame();
    }

    private void SpawnWithEffect(GameObject effectPrefab)
    {
        if (effectPrefab != null)
        {
            var effect = Instantiate(effectPrefab, cardSpawnPoint.position, Quaternion.identity);
            Destroy(effect, 2f); // Auto-cleanup
        }
    }

    public void DrawCard()
    {
        if (currentCategory == null || availableCards[currentCategory].Count == 0)
        {
            SelectCategory();
            return;
        }

        SpawnWithEffect(cardRevealEffectPrefab);
        ReleaseCurrentCard();

        int randomIndex = Random.Range(0, availableCards[currentCategory].Count);
        currentCardObject = Instantiate(
            availableCards[currentCategory].Dequeue(),
            cardSpawnPoint.position,
            cardSpawnPoint.rotation
        );
    }

    private void ReleaseCurrentCard()
    {
        if (currentCardObject != null)
        {
            if (currentCategory != null && currentCategory.cardPool != null)
                currentCategory.cardPool.Release(currentCardObject);
            else
                Destroy(currentCardObject);
        }
    }

    public void ResetDeck()
    {
        ReleaseCurrentCard();
        currentCategory = null;
        InitializePools();
    }
}
