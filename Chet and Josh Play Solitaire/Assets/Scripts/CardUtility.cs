using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUtility
{
    public static Column ParentColumn { private set; get; }

    public static void OnPointerEnter (List<Card> cards)
    {
        SetSpriteColor(cards, Color.green);
    }

    public static void OnPointerDown (List<Card> cards)
    {
        GameController.Instance.TempColumn.AddListToColumn(cards);
        ParentColumn = cards[0].ParentColumn;
    }

    public static void OnPointerUp (List<Card> cards, Column column)
    {
        
    }

    public static void OnPointerExit (List<Card> cards)
    {
        SetSpriteColor(cards, Color.white);
    }

    private static void SetSpriteColor (List<Card> cards, Color color)
    {
        foreach (Card card in cards)
        {
            card.SetSpriteColor(color);
        }
    }
}
