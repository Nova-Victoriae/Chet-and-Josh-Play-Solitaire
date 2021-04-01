using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUtility
{
    private static Column ParentColumn { get; set; }

    public static bool AreCardsEqual (Card a, Card b)
    {
        var suitsEqual = a.Suit == b.Suit;
        var ranksEqual = a.Rank == b.Rank;

        return suitsEqual && ranksEqual;
    }

    public static void OnPointerEnter (Card card)
    {
        var faceUp = card.ParentColumn.GetFaceUpCards();
        ColorCards(faceUp, Color.green);
    }

    public static void OnPointerExit (Card card)
    {
        var faceUp = card.ParentColumn.GetFaceUpCards();
        ColorCards(faceUp, Color.white);
    }

    public static void OnPointerDown (Card card)
    {
        // I HAVE NO IDEA HOW TO MAKE THIS WORK!!!!!!!!
        // I JUST NEED TO CHECK IF THE PLAYER IS PICKING UP A
        // CARD AND IF THEY ARE THEN MOVE THE CARD TO THE NEW COLUMN.
        // ELSE SET THE PLAYER TO PICKING UP A CARD AND ADD THE CARD
        // TO THE TEMP COLUMN.

        if (GameController.Instance.TempColumn.CardCount > 0) // If the temp column has cards in it.
        {
            if (card.ParentColumn.CanAddTo(GameController.Instance.TempColumn.Cards[0]))
            {
                Column temp = GameController.Instance.TempColumn.Cards[0].ParentColumn;

                card.ParentColumn.AddColumn(GameController.Instance.TempColumn);
                
                foreach(Card c in card.ParentColumn.Cards)
                {
                    c.ParentColumn = card.ParentColumn;
                }

                temp.AdjustSelf();
                temp.FlipTopCard();
                card.ParentColumn.AdjustSelf();
            }
            else
            {
                var c = GameController.Instance.TempColumn.Cards[0];
                c.ParentColumn.AddColumn(GameController.Instance.TempColumn);
                c.ParentColumn.AdjustSelf();
            }
        }
        else // The temp column has no cards in it.
        {
            // Add the card to the temp column.
            var faceUpCards = card.ParentColumn.GetFaceUpCardsAt(card);
            GameController.Instance.TempColumn.AddListToColumn(faceUpCards);
            
            foreach(Card c in faceUpCards)
            {
                card.ParentColumn.RemoveCardAt(card.ParentColumn.Cards.IndexOf(card));
            }
        }
    }

    private static void ColorCards (List<Card> cards, Color color)
    {
        foreach (Card card in cards)
        {
            card.SetSpriteColor(color);
        }
    }
}
