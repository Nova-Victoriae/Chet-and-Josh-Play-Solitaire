using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableauColumn : Column
{
    public override bool CanAddTo(Card card)
    {
        // Check to see if the column is empty and the card to add it is a king.
        // Test to see if the cards are of alternating suits.
        // Check to see if the top card of the column is the same as the card to add rank + 1.
        if (_cards.Count == 0 && card.Rank == Card.CardRank.KING)
        {
            return true;
        }

        if ((card.IsRed == !_cards[_cards.Count - 1].IsRed) || (!card.IsRed == _cards[_cards.Count - 1].IsRed))
        {
            if (_cards[_cards.Count - 1].Rank == card.Rank + 1)
            {
                return true;
            }
        }

        return false;
    }

    public override void RemoveCard(Card card)
    {
        base.RemoveCard(card);
        FlipTopCard();
    }
}
