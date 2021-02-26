using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The deck object.
/// </summary>
[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (BoxCollider2D))]
public class Deck : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab = null;

    private List<Card> _cards = new List<Card>();

    private const int _SUIT_SIZE = 4;
    private const int _RANK_SIZE = 13;

    private SpriteRenderer _spriteRenderer;

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

                card.DefineCard((Card.CardRank) (rank + 1), (Card.CardSuit) suit, data.DeckCardBack, data.DeckCardFrounts[((suit + 1) * (rank + 1)) - 1]);
                cardObj.name = card.ToString();
                _cards.Add(card);
            }
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

    // (int) DateTime.Now.Ticks & 0x0000FFFF
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
    /// The built in Unity Monobehaviour message <see href="https://docs.unity3d.com/2021.1/Documentation/ScriptReference/MonoBehaviour.Awake.html">Awake</see>
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
