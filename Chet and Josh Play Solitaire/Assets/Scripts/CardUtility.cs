using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUtility
{
    public static Column ParentColumn { private set; get; }

    /// <summary>
    /// Expands the OnPointerEnter in the Card class. This allows for a seperated funcationality
    /// for card selection.
    /// </summary>
    /// <param name="card">The card that called this method.</param>
    public static void OnPointerEnter (Card card)
    {
        // Get the card's column
        // Then get that card's index in the column.
        // Loop though the column starting at the card's index and color the cards until the end.
        var column = card.ParentColumn;
        var cardsIndex = column.Cards.IndexOf(card);

        switch (column.tag)
        {
            case "Waste":
                if (cardsIndex == column.CardCount - 1)
                {
                    card.ColorSprite(Color.red);
                }
                break;
            default:
                for (var i = cardsIndex; i < column.CardCount; i++)
                {
                    column.Cards[i].ColorSprite(Color.red);
                }
                break;
        }
    }

    public static void OnPointerDown(Card card)
    {
        var column = card.ParentColumn;

        switch (column.tag)
        {
            case "Waste":
                if (column.Cards.IndexOf(card) == column.CardCount - 1)
                {
                    SelectCard(card);
                }
                break;
            default:
                SelectCard(card);
                break;
        }
    }

    public static void OnPointerUp (Card card)
    {
        GameController.Instance.TempColumn.RemoveCard(card);
        ParentColumn.AddToColumn(card);
        ParentColumn.AdjustSelf();
        card.IsPickedUp = false;
        ParentColumn = null;
    }

    /// <summary>
    /// Expands the OnPointerExit in the Card class. This allows for a seperated funcationality
    /// for card selection.
    /// </summary>
    /// <param name="card">The card that called this method.</param>
    public static void OnPointerExit (Card card)
    {
        var column = card.ParentColumn;
        var cardsIndex = column.Cards.IndexOf(card);

        for (var i = cardsIndex; i < column.CardCount; i++)
        {
            column.Cards[i].ColorSprite(Color.white);
        }
    }

    private static void SelectCard (Card card)
    {
        ParentColumn = card.ParentColumn;
        card.IsPickedUp = true;
        card.ParentColumn.RemoveCard(card);
        GameController.Instance.TempColumn.AddToColumn(card);
        ParentColumn.AdjustSelf();
    }
}
