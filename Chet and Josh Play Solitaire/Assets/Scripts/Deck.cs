using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The deck object.
/// </summary>
[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (BoxCollider2D))]
public class Deck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private GameObject _cardPrefab = null;
    [SerializeField] private Color _highlightColor = Color.cyan;

    private List<Card> _cards = new List<Card>();

    private const int _SUIT_SIZE = 4;
    private const int _RANK_SIZE = 13;

    private SpriteRenderer _spriteRenderer = null;
    private Column _wasteColumn = null;

    /// <summary>
    /// Creates each card in the deck. Then parents the created card to the deck game object.
    /// </summary>
    /// <param name="data">The information that describes the deck.</param>
    public void CreateDeck (DeckData data)
    {
        // Sets the deck sprite (the card's back).
        // Creates each card.
        // Sets each card active self to false.

        _spriteRenderer.sprite = data.DeckCardBack;

        var linearCount = 0;
        // This feels like I'm going to break the universe not starting the for loop with i, j, x, y, ...
        for (var suit = 0; suit < _SUIT_SIZE; suit++)
        {
            for (var rank = 0; rank < _RANK_SIZE; rank++)
            {
                var cardObj = Instantiate(_cardPrefab, transform.position, Quaternion.identity, transform);
                var card = cardObj.GetComponent<Card>();

                if (card == null)
                {
                    Debug.LogError("There is no card script attaced to the card prefab. Attach the card script to the card prefab.");
                }

                //Debug.LogFormat("Index: {0}, Sprite Name: {1}", ((suit + 1) * (rank + 1)) - 1, data.DeckCardFrounts[((suit + 1) * (rank + 1)) - 1].name);
                Debug.LogFormat("Linear Count: {0}", linearCount);
                card.DefineCard((Card.CardRank) (rank), (Card.CardSuit) suit, data.DeckCardFrounts[linearCount], data.DeckCardBack);
                cardObj.name = card.ToString();
                _cards.Add(card);

                cardObj.SetActive(false);
                linearCount += 1;
            }
            //linearCount += 1;
        }
    }

    /// <summary>
    /// Deals the top card of the deck. NOTE: The convention is that the top of the list/array is the last entry of the array.
    /// </summary>
    /// <returns>The top card of the deck.</returns>
    public Card Deal ()
    {
        Card card = _cards[_cards.Count - 1];
        _cards.RemoveAt(_cards.Count - 1);

        return card;
    }

    /// <summary>
    /// Deals any amount of cards and stores them in a list.
    /// </summary>
    /// <param name="amount">The amount of cards to deal.</param>
    /// <returns></returns>
    public List<Card> DealCards (int amount)
    {
        var cards = new List<Card>();

        for (var i = 0; i < amount; i++)
        {
            cards.Add(Deal());
        }

        return cards;
    }

    /// <summary>
    /// Deals the game. Places cards in each tableau column.
    /// </summary>
    /// <param name="tableauColumns">The list of tableau columns.</param>
    /// <param name="cardsToDeal">The amount of cards to deal. Defaults to 28 cards.</param>
    public void DealGame (List<TableauColumn> tableauColumns, int cardsToDeal = 28)
    {
        var startingIndex = 0;

        for (var i = 0; i < cardsToDeal; i++)
        {
            for (var index = startingIndex; index < tableauColumns.Count; index++)
            {
                //Debug.LogFormat("I_S: {0} - I: {1} - i: {2}", startingIndex, index, i);
                var card = Deal();
                tableauColumns[index].AddToColumn(card);
                card.transform.position = tableauColumns[index].transform.localPosition;
                card.ParentColumn = tableauColumns[index];
                card.gameObject.SetActive(true);
            }

            startingIndex++;
        }
    }

    /// <summary>
    /// Shuffles the deck with the given seed.
    /// </summary>
    /// <param name="seed">The seed for the random number generator Use: (int) DateTime.Now.Ticks & 0x0000FFFF for a random seed</param>
    public void ShuffleDeck(int seed)
    {
        System.Random random = new System.Random(seed);

        for (var i = 0; i < _cards.Count - 1; i++)
        {
            int rndIndex = random.Next(i, _cards.Count);
            Card tempCard = _cards[rndIndex];
            _cards[rndIndex] = _cards[i];
            _cards[i] = tempCard;
        }
    }

    /// <summary>
    /// The call back for when the pointer enters the object. 
    /// <see href="https://docs.unity3d.com/2018.4/Documentation/ScriptReference/EventSystems.IPointerEnterHandler.html">Unity Documentation</see> 
    /// </summary>
    /// <param name="eventData">
    /// The <see href="https://docs.unity3d.com/2018.4/Documentation/ScriptReference/EventSystems.PointerEventData.html">pointer event data</see>
    /// </param>
    public void OnPointerEnter (PointerEventData data)
    {
        _spriteRenderer.color = _highlightColor;
    }

    /// <summary>
    /// The call back for when the pointer clicks the object. 
    /// <see href="https://docs.unity3d.com/2018.4/Documentation/ScriptReference/EventSystems.IPointerDownHandler.html">Unity Documentation</see> 
    /// </summary>
    /// <param name="eventData">
    /// The <see href="https://docs.unity3d.com/2018.4/Documentation/ScriptReference/EventSystems.PointerEventData.html">pointer event data</see>
    /// </param>
    public void OnPointerDown (PointerEventData data)
    {
        // If the deck is empty then add the waste column to the deck and clear the waste column.
        // Else adds three cards to the waste pile.

        if (_cards.Count == 0)
        {
            var wasteCount = _wasteColumn.CardCount;

            for (var i = 0; i < wasteCount; i++)
            {
                var card = _wasteColumn.PopTop();

                card.transform.parent = transform;
                card.transform.position = transform.position;
                card.ParentColumn = null;
                card.gameObject.SetActive(false);
                card.Flip();

                _cards.Add(card);
            }
        }
        else
        {
            var cardsToDeal = (_cards.Count < 3) ? _cards.Count : 3; // If the amount of cards to deal is less than 3 just deal the remainder of the deck.

            for (var i = 0; i < cardsToDeal; i++)
            {
                var card = Deal();
                card.gameObject.SetActive(true);
                card.transform.position = _wasteColumn.transform.position;
                card.ParentColumn = _wasteColumn;
                card.Flip();
                _wasteColumn.AddToColumn(card);
            }

            _wasteColumn.AdjustSelf();
        }
    }

    /// <summary>
    /// The call back for when the pointer exits the object. 
    /// <see href="https://docs.unity3d.com/2018.4/Documentation/ScriptReference/EventSystems.IPointerExitHandler.html">Unity Documentation</see>
    /// </summary>
    /// <param name="eventData">
    /// The <see href="https://docs.unity3d.com/2018.4/Documentation/ScriptReference/EventSystems.PointerEventData.html">pointer event data</see>
    /// </param>
    public void OnPointerExit (PointerEventData data)
    {
        _spriteRenderer.color = Color.white;
    }

    /// <summary>
    /// The built in Unity Monobehaviour message <see href="https://docs.unity3d.com/2021.1/Documentation/ScriptReference/MonoBehaviour.Awake.html">Awake</see>
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// The built in Unity Monobehaviour message <see href="https://docs.unity3d.com/2021.1/Documentation/ScriptReference/MonoBehaviour.Start.html">Awake</see>
    /// </summary>
    private void Start()
    {
        _wasteColumn = GameObject.FindGameObjectWithTag("Waste").GetComponentInChildren<Column>();
    }
}
