using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteColumn : Column
{
    /// <summary>
    /// Adjusts the wastes column. Maybe I should move this into the column class and maybe get rid of this class?
    /// Seems like the every column should work like this.
    /// </summary>
    
    public override List<Card> GetFaceUpCards ()
    {
        var faceUpCards = new List<Card>();
        faceUpCards.Add(_cards[_cards.Count - 1]);
        return faceUpCards;
    }
}
