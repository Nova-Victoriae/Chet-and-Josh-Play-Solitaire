using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a temp. Game Controller.
/// </summary>
public class GameController : MonoBehaviour
{
    public static GameController Instance { get { return _instance; } }

    public Column TempColumn { get; private set; }

    [SerializeField] private Color _background = Color.green;

    [SerializeField] private GameObject _deckPrefab = null;
    [SerializeField] private DeckData _deckData = null;

    private Transform _deckLocation = null;
    private List<TableauColumn> _tableauColumns = new List<TableauColumn>();

    private static GameController _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        Camera.main.backgroundColor = _background;
        TempColumn = GameObject.FindGameObjectWithTag("Temp Column").GetComponent<Column>();

        _deckLocation = GameObject.FindGameObjectWithTag("Deck").transform;
        
        foreach (Transform child in GameObject.FindGameObjectWithTag("Tableau").transform)
        {
            _tableauColumns.Add(child.GetComponent<TableauColumn>());
        }

        _tableauColumns.Reverse();

        Deck deck = SetUpDeck();
        deck.DealGame(_tableauColumns);

        foreach (TableauColumn tableauColumn in _tableauColumns)
        {
            tableauColumn.AdjustSelf();
            tableauColumn.FlipTopCard();
        }
    }

    /// <summary>
    /// Sets up the deck for the game.
    /// </summary>
    private Deck SetUpDeck ()
    {
        if (_deckPrefab != null && _deckData != null && _deckLocation != null)
        {
            var deckObj = Instantiate(_deckPrefab, transform.position, Quaternion.identity, _deckLocation);

            deckObj.name = "Deck";
            deckObj.transform.position = _deckLocation.position;

            var deck = deckObj.GetComponent<Deck>();

            if (deck == null)
            {
                Debug.LogError("The deck prefab must have the deck script attached to it. Attach the deck script to the deck prefab.");
            }

            deck.CreateDeck(_deckData);
            deck.ShuffleDeck((int)System.DateTime.Now.Ticks & 0x0000FFFF);

            return deck;
        }
        else
        {
            Debug.LogErrorFormat("Deck Prefab: {0} - Deck Data: {1}", (_deckPrefab != null) ? _deckPrefab.name : "null", (_deckData != null) ? _deckData.DeckName : "null");
        }

        return null;
    }
}
