using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public Card[] deck = new Card[52]; // Initializes an array of 52 Card objects
    public List<Card> player_deck = new List<Card>();
    public List<Card> ai_deck = new List<Card>();
    public List<Card> player_hand = new List<Card>();
    public List<Card> ai_hand = new List<Card>();
    public List<Card> discard_pile = new List<Card>();

    public Vector3 startPosition;
    public float xOffset;

    private void Awake()
    {
        if (gm != null && gm != this)
        {
            Destroy(gameObject);
        }
        else
        {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeDeck();
        Shuffle();
        Deal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeDeck()
    {
        // Populate the deck with card data
        for (int i = 0; i < deck.Length; i++)
        {
            deck[i] = Instantiate(new Card(), Vector3.zero, Quaternion.identity); // Replace with actual card initialization
        }
    }

    void Deal()
    {
        Vector3 currentPosition = startPosition;
        for (int i = 0; i < 5; i++)
        {
            // Deal a card to the player
            Card playerCard = Instantiate(deck[0], currentPosition, Quaternion.identity);
            player_hand.Add(playerCard);
            RemoveCardFromDeck(0);
            currentPosition.x += xOffset;

            // Deal a card to the AI
            Card aiCard = Instantiate(deck[0], currentPosition, Quaternion.identity);
            ai_hand.Add(aiCard);
            RemoveCardFromDeck(0);
            currentPosition.x += xOffset;
        }
    }

    void Shuffle()
    {
        for (int i = deck.Length - 1; i > 0; i--)
        {
            
            int randomIndex = Random.Range(0, i + 1);
            Card temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    void RemoveCardFromDeck(int index)
    {
        for (int i = index; i < deck.Length - 1; i++)
        {
            deck[i] = deck[i + 1];
        }
        deck[deck.Length - 1] = null;
    }

    void AI_Turn()
    {
        // AI turn logic here
    }
}

// Example Card class definition
/*public class Card : MonoBehaviour
{
    // Card properties and methods here
}
*/