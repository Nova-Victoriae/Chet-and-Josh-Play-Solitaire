using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Scriptable Object for the deck settings. It allows for the creation of mulitible deck settings.
/// </summary>
[CreateAssetMenu(fileName = "Card Data", menuName = "Scriptable Objects/Card Data", order = 1)]
public class DeckData : ScriptableObject
{
    public string DeckName { get { return _deckName; } }
    public Sprite DeckCardBack { get { return _deckCardBack; } }
    public List<Sprite> DeckCardFrounts { get { return _deckCardFrounts; } }

    [SerializeField] private string _deckName = "New Deck";
    [SerializeField] private Sprite _deckCardBack = null;
    [SerializeField] private List<Sprite> _deckCardFrounts = new List<Sprite>();
}
