using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationColumn : Column
{
    [SerializeField] private Card.CardSuit suit = Card.CardSuit.SPADES;

    public override bool CanAddTo(Card card)
    {
        if (_cards.Count <= 0 && card.Suit == suit && card.Rank == Card.CardRank.ACE)
        {
            return true;
        }

        if (card.Rank == _cards[_cards.Count - 1].Rank + 1 && card.Suit == suit)
        {
            return true;
        }

        return false;
    }
}
