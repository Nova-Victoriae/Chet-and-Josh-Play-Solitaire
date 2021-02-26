using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a temp. Game Controller. TODO: make singleton.
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _deckPrefab = null;
    [SerializeField] private DeckData _deckData = null;

    private void Start()
    {
        if (_deckPrefab != null && _deckData != null)
        {
            var deckObj = Instantiate(_deckPrefab, transform.position, Quaternion.identity);
            deckObj.name = "deck";

            var deck = deckObj.GetComponent<Deck>();

            if (deck == null)
            {
                Debug.LogError("The deck prefab must have the deck script attached to it. Attach the deck script to the deck prefab.");
            }

            deck.CreateDeck(_deckData);
            deck.ShuffleDeck((int) System.DateTime.Now.Ticks & 0x0000FFFF);

            // Test
            for (var i = 0; i < 52; i++)
            {
                var card = deck.Deal();
                Debug.LogFormat("Card Name: {0}", card.ToString());
            }
        }
        else
        {
            Debug.LogErrorFormat("Deck Prefab: {0} - Deck Data: {1}", (_deckPrefab != null) ? _deckPrefab.name : "null", (_deckData != null) ? _deckData.DeckName : "null");
        }
    }
}
