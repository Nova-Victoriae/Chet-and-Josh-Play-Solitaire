using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationColumn : Column
{
    [SerializeField] private Card.CardSuit suit = Card.CardSuit.SPADES;

    public override void AddColumn(Column column)
    {
        if (IsEmpty && column.Cards[0].Suit == suit && column.Cards[0].Rank == Card.CardRank.ACE)
        {
            _cards.Add(column.PopTop());
        }
    }
}
