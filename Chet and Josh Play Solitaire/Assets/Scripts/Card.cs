using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The repsentation of cards in the game.
/// </summary>
[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (BoxCollider2D))]
public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum CardSuit { SPADES, DIAMONDS, CLUBS, HEARTS }
    public enum CardRank { ACE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING }

    public CardSuit Suit { get; private set; }
    public CardRank Rank { get; private set; }
    public bool IsFacingUp { get; set; } = false;
    public bool IsRed { get { return Suit == CardSuit.DIAMONDS || Suit == CardSuit.HEARTS; } }
    public bool IsPickedUp { get; set; }
    public Column ParentColumn { get; set; }

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
        Debug.LogFormat("IsFacingUp: {0} - PlayerPickingUpCard: {1}", IsFacingUp, GameController.Instance.PlayerPickingUpCard);
        if (IsFacingUp && !GameController.Instance.PlayerPickingUpCard)
            CardUtility.OnPointerEnter(this);
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        if (IsFacingUp)
        {
            CardUtility.OnPointerDown(this);
        }
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        if (IsFacingUp)
        {
            var columns = Physics2D.CircleCastAll(transform.position, 0.5f, Vector2.one);
            Column temp = null;

            foreach (var column in columns)
            {
                temp = column.transform.GetComponent<Column>();

                if (temp != null)
                {
                    break;
                }
            }

            CardUtility.OnPointerUp(this, temp);
        }
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
        if (IsFacingUp && !GameController.Instance.PlayerPickingUpCard)
            CardUtility.OnPointerExit(this);
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

    public void ColorSprite (Color color)
    {
        _spriteRenderer.color = color;
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
    /// Equality comparison of two cards.
    /// </summary>
    /// <param name="card">The card to see if it equal to this card.</param>
    /// <returns>Returns true if the cards suits and ranks are equal.</returns>
    public bool IsEqual (Card card)
    {
        return card.Suit == Suit && card.Rank == Rank;
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

    private void Update()
    {
        if (IsPickedUp)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
        }
    }
}
