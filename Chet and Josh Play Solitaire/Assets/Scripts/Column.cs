using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The columns of the deck. This is the most basic type of column. Really just a fancy list of cards.
/// </summary>
public class Column : MonoBehaviour
{
    public int CardCount { get { return _cards.Count; } }
    public bool IsEmpty { get { return (_cards.Count == 0) ? true : false; } }
    public List<Card> Cards { get { return _cards; } }
    [SerializeField] protected Vector3 _offset = Vector3.zero;
    [SerializeField] protected int _totalCardsToShow = 9;



    protected List<Card> _cards = new List<Card>();

    /// <summary>
    /// Gets all the face up cards in the column. This should be the same for all columns.
    /// The WasteColumn, Tableau Column, and Foundation Column, and Base Column.
    /// </summary>
    /// <returns>
    /// All the face up cards in the column. Should be a copy of the cards in the column. 
    /// Should be override in the Tableau Column as that is the only column with face down cards.
    /// </returns>
    public virtual List<Card> GetFaceUpCards ()
    {
        var faceUpCards =  new List<Card>();

        foreach (Card card in _cards)
        {
            if (card.IsFacingUp)
                faceUpCards.Add(card);
        }

        return faceUpCards;
    }

    /// <summary>
    /// Flips the top card of the column.
    /// </summary>
    public void FlipTopCard ()
    {
        if (_cards.Count == 0)
        {
            return;
        }
        
        _cards[_cards.Count - 1].Flip();
    }

    /// <summary>
    /// Adds the card to the column.
    /// </summary>
    /// <param name="card">The card to add to the column</param>
    public virtual void AddToColumn (Card card)
    {
        _cards.Add(card);
        card.transform.parent = transform;
    }

    /// <summary>
    /// Adds the card at the index to the column.
    /// </summary>
    /// <param name="card">The card to add.</param>
    /// <param name="index">The index to add the card to.</param>
    public virtual void AddToColumnAt (Card card, int index)
    {
        _cards.Insert(index, card);
        card.transform.parent = transform;
    }

    /// <summary>
    /// Adds the cards from a column to the end of this column.
    /// </summary>
    /// <param name="column">The column to add to this column.</param>
    public virtual void AddColumn (Column column)
    {
        for (var i = 0; i < column.CardCount; i++)
        {
            _cards.Add(column.PopCardAt(i));
        }

        foreach (Card card in _cards)
        {
            card.transform.parent = transform;
        }
    }

    public virtual void AddListToColumn (List<Card> cards)
    {
        for(var i = 0; i < cards.Count; i++)
        {
            AddToColumn(cards[i]);
            cards[i].transform.parent = transform;
        }
    }

    /// <summary>
    /// Removes the card from the column.
    /// </summary>
    /// <param name="card">The card to remove.</param>
    public virtual void RemoveCard (Card card)
    {
        _cards.Remove(card);
    }

    /// <summary>
    /// Remove a card from the column at the index.
    /// </summary>
    /// <param name="index">The index </param>
    public virtual void RemoveCardAt (int index)
    {
        _cards.RemoveAt(index);
    }

    /// <summary>
    /// Pops (removes) the card at the index.
    /// </summary>
    /// <param name="index">The index of the card to be popped.</param>
    /// <returns>The popped card.</returns>
    public virtual Card PopCardAt (int index)
    {
        var poppedCard = _cards[index];
        _cards.Remove(_cards[index]);

        return poppedCard;
    }

    /// <summary>
    /// Pops the frist card (index = 0) of the column.
    /// </summary>
    /// <returns>The card at index 0.</returns>
    public virtual Card PopBottom ()
    {
        return PopCardAt(0);
    }

    /// <summary>
    /// Pops the top card (last element of the list) of the column.
    /// </summary>
    /// <returns>The card at the last index of the list.</returns>
    public virtual Card PopTop ()
    {
        return PopCardAt(CardCount - 1);
    }

    /// <summary>
    /// Adjusts each card in the column.
    /// </summary>
    public virtual void AdjustSelf ()
    {
        if (_cards.Count > 0)
        {
            for (var i = 0; i < _cards.Count; i++)
            {
                var indexToStartOffsetAt = CardCount - _totalCardsToShow; // Todo think of better name.

                if (i > indexToStartOffsetAt || _cards.Count < _totalCardsToShow)
                {
                    var adjustedIndex = (_cards.Count < _totalCardsToShow) ? i : i - indexToStartOffsetAt;
                    _cards[i].transform.position = transform.position + (_offset * adjustedIndex);
                }
                else
                {
                    _cards[i].transform.position = transform.position;
                }

                _cards[i].SetOrderSorting(i);
            }
        }

        //FlipTopCard();
    }

    /// <summary>
    /// If the given card can be added to the column.
    /// </summary>
    /// <param name="card">The card to be see if can be added to the column.</param>
    /// <returns>True if the card can be added to the column.</returns>
    public virtual bool CanAddTo (Card card)
    {
        return true;
    }
}
