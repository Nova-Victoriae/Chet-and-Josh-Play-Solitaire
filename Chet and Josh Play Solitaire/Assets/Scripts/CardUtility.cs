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
    }

    private static void ColorCards (List<Card> cards, Color color)
    {
        foreach (Card card in cards)
        {
            card.SetSpriteColor(color);
        }
    }
}
