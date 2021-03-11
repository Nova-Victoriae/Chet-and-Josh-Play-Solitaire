using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUtility
{
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
        // If there are cards in the temp column
        //      Attempt to move them to this column.
        // Else
        //      Add all face up cards to the temp column.

        if (GameController.Instance.TempColumn.CardCount > 0)
        {
            var column = card.ParentColumn;
            var oldParent = GameController.Instance.TempColumn.Cards[0].ParentColumn;

            if (column.CanAddTo(GameController.Instance.TempColumn.Cards[0]))
            {
                Debug.LogFormat("Adding card to the new column");
                column.AddColumn(GameController.Instance.TempColumn);
            }
            else
            {
                Debug.LogFormat("Adding card to the old column");
                oldParent.AddColumn(GameController.Instance.TempColumn);
            }

            column.AdjustSelf();
            oldParent.AdjustSelf();
        }
        else
        {
            var faceUpCards = card.ParentColumn.GetFaceUpCards();
            GameController.Instance.TempColumn.AddListToColumn(faceUpCards);
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
