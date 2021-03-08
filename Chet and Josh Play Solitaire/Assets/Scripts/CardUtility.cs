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

        if (column == null)
        {
            return;
        }

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

    public static void OnPointerUp (Card card, Column columnToAdd)
    { 
        DeselectCard(card, columnToAdd);
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

        //Debug.LogFormat("Card's Index: {0}", cardsIndex);

        if (cardsIndex < 0)
        {
            return;
        }

        for (var i = cardsIndex; i < column.CardCount; i++)
        {
            column.Cards[i].ColorSprite(Color.white);
        }
    }

    private static void SelectCard (Card card)
    {
        var column = card.ParentColumn;
        var cardsIndex = column.Cards.IndexOf(card);

        GameController.Instance.PlayerPickingUpCard = true;
        ParentColumn = column;

        for (var i = cardsIndex; i < column.CardCount; i++)
        {
            var c = column.Cards[i];
            c.IsPickedUp = true;
            column.RemoveCard(c);
            GameController.Instance.TempColumn.AddToColumn(c);
        }

        ParentColumn.AdjustSelf();
    }

    private static void DeselectCard (Card card, Column column)
    {
        GameController.Instance.PlayerPickingUpCard = false;

        if (column == null)
        {
            //AddCardToColumn(card, ParentColumn);
            AddColumnToColumn(ParentColumn);
            return;
        }

        switch (column.tag)
        {
            case "Foundation":
                if (column.CanAddTo(card))
                {
                    //AddCardToColumn(card, column, true);
                    AddColumnToColumn(column, true);
                }
                else
                {
                    //AddCardToColumn(card, ParentColumn);
                    AddColumnToColumn(ParentColumn);
                }

                break;
            case "Tableau":
                if (column.CanAddTo(card))
                {
                    //AddCardToColumn(card, column, true);
                    AddColumnToColumn(column, true);
                }
                else
                {
                    //AddCardToColumn(card, ParentColumn);
                    AddColumnToColumn(ParentColumn);
                }

                break;
            default:
                //AddCardToColumn(card, ParentColumn);
                AddColumnToColumn(ParentColumn);
                break;
        }

    }

    private static void AddColumnToColumn (Column column, bool flipTopCard = false)
    {
        foreach (var card in column.Cards)
        {
            card.ParentColumn = column;
            card.transform.parent = card.ParentColumn.transform;
            card.IsPickedUp = false;
        }

        if (flipTopCard && !ParentColumn.tag.Equals("Waste"))
        {
            ParentColumn.FlipTopCard();
        }

        ParentColumn = null;
        column.Cards[0].ParentColumn.AddColumn(GameController.Instance.TempColumn);
        column.Cards[0].ParentColumn.AdjustSelf();
    }

    /// <summary>
    /// Adds the card to a column.
    /// </summary>
    /// <param name="card">The card that called this method.</param>
    /// <param name="card">The column to add the card to.</param>
    private static void AddCardToColumn (Card card, Column column, bool flipTopCard = false)
    {
        card.ParentColumn = column;
        card.transform.parent = card.ParentColumn.transform;
        card.IsPickedUp = false;

        if (flipTopCard && !ParentColumn.tag.Equals("Waste"))
        {
            ParentColumn.FlipTopCard();
        }

        ParentColumn = null;
        card.ParentColumn.AddColumn(GameController.Instance.TempColumn);
        card.ParentColumn.AdjustSelf();
    }
}
