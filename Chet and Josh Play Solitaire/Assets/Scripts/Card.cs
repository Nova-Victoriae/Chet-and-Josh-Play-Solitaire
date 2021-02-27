using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The repsentation of cards in the game.
/// </summary>
[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (BoxCollider2D))]
public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum CardSuit { SPADES, DIAMONDS, CLUBS, HEARTS }
    public enum CardRank { ACE = 1, TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8, NINE = 9, TEN = 10, JACK = 11, QUEEN = 12, KING = 13 }

    public CardSuit Suit { get; private set; }
    public CardRank Rank { get; private set; }
    public bool IsFacingUp { get; set; } = false;
    public bool IsRed { get { return Suit == CardSuit.DIAMONDS || Suit == CardSuit.HEARTS; } }

    private SpriteRenderer _spriteRenderer = null;
    private Sprite _faceSprite = null;
    private Sprite _backSprite = null;
    [SerializeField] private Color _highLightColor = Color.cyan;

    /// <summary>
    /// Define's the card's Suit, Rank, and sprites (face and back.).
    /// </summary>
    /// <param name="rank">The card's rank (SPADES, DIAMONDS, CLUBS, HEARTS)</param>
    /// <param name="suit">The card's suit (ACE, TWO, ..., QUEEN, KING)</param>
    /// <param name="face">The card's face sprite/image.</param>
    /// <param name="back">The card's back sprite/image.</param>
    public void DefineCard (CardRank rank, CardSuit suit, Sprite face, Sprite back)
    {
        Rank = rank;
        Suit = suit;

        _faceSprite = face;
        _backSprite = back;
    }

    /// <summary>
    /// The call back for when the pointer enters the object. 
    /// <see href="https://docs.unity3d.com/2018.4/Documentation/ScriptReference/EventSystems.IPointerEnterHandler.html">Unity Documentation</see> 
    /// </summary>
    /// <param name="eventData">
    /// The <see href="https://docs.unity3d.com/2018.4/Documentation/ScriptReference/EventSystems.PointerEventData.html">pointer event data</see>
    /// </param>
    public void OnPointerEnter (PointerEventData eventData)
    {
        _spriteRenderer.color = _highLightColor;
    }

    /// <summary>
    /// The call back for when the pointer exits the object. 
    /// <see href="https://docs.unity3d.com/2018.4/Documentation/ScriptReference/EventSystems.IPointerExitHandler.html">Unity Documentation</see>
    /// </summary>
    /// <param name="eventData">
    /// The <see href="https://docs.unity3d.com/2018.4/Documentation/ScriptReference/EventSystems.PointerEventData.html">pointer event data</see>
    /// </param>
    public void OnPointerExit (PointerEventData eventData)
    {
        _spriteRenderer.color = Color.white;
    }

    /// <summary>
    /// Flips the card.
    /// </summary>
    public void Flip ()
    {
        IsFacingUp = !IsFacingUp;

        if (IsFacingUp)
            _spriteRenderer.sprite = _faceSprite;
        else
            _spriteRenderer.sprite = _backSprite;
    }

    /// <summary>
    /// Sets the sorting order of the card.
    /// </summary>
    /// <param name="order">The sorting order to set.</param>
    public void SetOrderSorting (int order)
    {
        _spriteRenderer.sortingOrder = order;
    }

    /// <summary>
    /// The override of the Object ToString().
    /// </summary>
    /// <returns>The a string of the cards name Rank_of_Suit lower cased.</returns>
    public override string ToString()
    {
        return (Rank.ToString() + "_of_" + Suit.ToString()).ToLower();
    }

    /// <summary>
    /// The built in Unity Monobehaviour message <see href="https://docs.unity3d.com/2021.1/Documentation/ScriptReference/MonoBehaviour.Awake.html">Awake</see>
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
