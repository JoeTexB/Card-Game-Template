using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public Card[] deck = new Card[52];// Initializes an array of 52 Card objects
    public Card[] jokerdeck = new Card[6];
    public List<Card> player_hand = new List<Card>();
    public List<Card> joker_hand = new List<Card>();
    public List<Card> ai_hand = new List<Card>();
    public List<Card> discard_pile = new List<Card>();

    public Vector3 startPosition;
    public float xOffset;
    public GameObject cardPrefab; // Reference to the Card prefab
    public Transform canvasTransform; // Reference to the Canvas transform

    public Vector3 Canvas;

    public Vector2 VAiHand;

    public Vector2 VPlayerHand;

    public Vector2 CardShift;

    public Card PlayerCard;

    public Card AiCard;

    public bool PlayerAce;

    public bool AiAce;

    public int PlayerTotal;

    public int AiTotal;

    public bool AiTurn;

    public GameObject EndGame;

    public GameObject JokerHitButton;

    public string WinnerText;

    public bool PlayerS;

    public bool AiS;

    public string jokers;

    private void Awake()
    {
        Debug.Log("Awake method called.");
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

        VAiHand = new Vector2(-300, 600);

        VPlayerHand = new Vector2(-300, 220);

        CardShift = new Vector2(180, 0);

        PlayerTotal = 0;

        AiTotal = 0;

        AiTurn = false;

        PlayerS = false;

        AiS = false;

        PlayerAce = false;

        AiAce = false;

        

        EndGame = GameObject.Find("EndGame");
        EndGame.SetActive(false);
        JokerHitButton = GameObject.Find("JokerHit");
        JokerHitButton.SetActive(false);

        Debug.Log("Start method called.");
        InitializeDeck();
        Shuffle(); // Shuffle the deck before dealing
        Deal();
        
        Canvas = canvasTransform.position;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerTotal < AiTotal)
        {
            if (PlayerTotal > 16)
            {
                if (AiTotal <= 21)
                {
                    JokerHitButton.SetActive(true);
                }
                
            }
        }
        {

        }
        if (PlayerTotal > 21)
        {
            if (PlayerAce == true)
            {
                PlayerTotal -= 10;
                PlayerAce = false;
            }
            else
            {
                PlayerBust();
            }

        }
        if (AiTotal > 21)
        {
            if (AiAce == true)
            {
                AiTotal -= 10;
                AiAce = false;
            }
            else
            {
                AiBust();
            }

        }
        if (PlayerS == true && AiS == true)
        {
            if (PlayerTotal > AiTotal)
            {
                if (PlayerTotal <= 21)
                {
                    EndGame.SetActive(true);
                    WinnerText = "Player Wins!";
                }
            }
            if (AiTotal > PlayerTotal)
            {
                if (AiTotal <= 21)
                {
                    EndGame.SetActive(true);
                    WinnerText = "Ai Wins!";
                }
            }
            if (AiTotal == PlayerTotal)
            {
                EndGame.SetActive(true);
                WinnerText = "Draw, Ai Wins!";
                JokerHitButton.SetActive(true);
            }
        }
        if (jokers == "JOE" || jokers == "EOJ")
        {
            EndGame.SetActive(true);
            WinnerText = jokers + " Player Wins!";
        }
        {

        }
        

    }

    void InitializeDeck()
    {
        Debug.Log("InitializeDeck method called.");
        // Populate the deck with card data
        for (int i = 0; i < deck.Length; i++)
        {
            if (cardPrefab != null)
            {
                deck[i] = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, canvasTransform).GetComponent<Card>(); // Instantiate as child of Canvas
                Debug.Log("Card " + i + " initialized.");
            }
            else
            {
                Debug.LogError("Card prefab is not assigned.");
            }
        }

    }

    void Deal()
    {
        Debug.Log("Deal method called.");
        Vector3 currentPosition = startPosition;
        for (int i = 0; i < 2; i++) // Deal 2 cards to each player
        {
            // Deal a card to the player
            if (deck.Length > 0 && deck[0] != null)
            {
                Card playerCard = deck[0];
                playerCard.transform.SetParent(canvasTransform, false); // Set the parent to the Canvas
                playerCard.transform.position = currentPosition;
                player_hand.Add(playerCard);
                Debug.Log("Player card added to hand: " + playerCard.name);
                RemoveCardFromDeck(0);
                currentPosition.x += xOffset;
                
            }
            else
            {
                Debug.LogError("No cards left in the deck to deal to the player.");
            }

            // Deal a card to the AI
            if (deck.Length > 0 && deck[0] != null)
            {
                Card aiCard = deck[0];
                aiCard.transform.SetParent(canvasTransform, false); // Set the parent to the Canvas
                aiCard.transform.position = currentPosition;
                ai_hand.Add(aiCard);
                Debug.Log("AI card added to hand: " + aiCard.name);
                RemoveCardFromDeck(0);
                currentPosition.x += xOffset;
               
            }
            else
            {
                Debug.LogError("No cards left in the deck to deal to the AI.");

            }
        }

        // Log the contents of the player and AI hands
        Debug.Log("Player hand contents:");
        foreach (Card card in player_hand)
        {
            PlayerCard = card;
            PlayerCardInstantiate();
            
            
        }

        Debug.Log("AI hand contents:");
        foreach (Card card in ai_hand)
        {
            AiCard = card;
            AiCardInstantiate();
            
            

        }
    }

    void Shuffle()
    {
        Debug.Log("Shuffle method called.");
        // Fisher-Yates shuffle algorithm
        for (int i = deck.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Card temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        for (int i = jokerdeck.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Card temp = jokerdeck[i];
            jokerdeck[i] = jokerdeck[randomIndex];
            jokerdeck[randomIndex] = temp;
        }
    }

    void RemoveCardFromDeck(int index)
    {
        Debug.Log("RemoveCardFromDeck method called.");
        // Shift all cards to the left to remove the card at the specified index
        for (int i = index; i < deck.Length - 1; i++)
        {
            deck[i] = deck[i + 1];
        }
        deck[deck.Length - 1] = null;
        Debug.Log("Card removed from deck at index: " + index);
    }

    void RemoveCardFromJokerDeck(int index)
    {
        Debug.Log("RemoveCardFromDeck method called.");
        // Shift all cards to the left to remove the card at the specified index
        for (int i = index; i < jokerdeck.Length - 1; i++)
        {
            jokerdeck[i] = jokerdeck[i + 1];
        }
        jokerdeck[jokerdeck.Length - 1] = null;
        Debug.Log("Card removed from jokerdeck at index: " + index);
    }


    private void PlayerCardInstantiate()
    {
        Debug.Log(PlayerCard.name);
        GameObject newCard = Instantiate(PlayerCard, Canvas, Quaternion.identity).gameObject;
        newCard.transform.parent = canvasTransform;
        newCard.transform.position = VPlayerHand /* new Vector2(0, 0)*/;
        VPlayerHand.x += CardShift.x;
        PlayerTotal += PlayerCard.data.cost;
        if (PlayerCard.data.cost == 11)
        {
            PlayerAce = true;
        }
    }
    private void AiCardInstantiate()
    {
        Debug.Log(AiCard.name);
        GameObject newCard = Instantiate(AiCard, Canvas, Quaternion.identity).gameObject;
        newCard.transform.parent = canvasTransform;
        newCard.transform.position = VAiHand /*new Vector2(0, 0)*/;
        VAiHand.x += CardShift.x;
        AiTotal += AiCard.data.cost;
        if (AiCard.data.cost == 11)
        {
            AiAce = true;
        }
    }

    public void PlayerHit()
    {
        Debug.Log("Player hits!");
        Card playerCard = deck[0];
        player_hand.Add(playerCard);
        Debug.Log("Player card added to hand: " + playerCard.name);
        RemoveCardFromDeck(0);
        PlayerCard = playerCard;
        PlayerCardInstantiate();
        
        
    }

  
    public void PlayerStand()
    {
        Debug.Log("Player stands!");
        AiTurn = true;
        PlayerS = true;
        
    }

    public void PlayerHitJoker()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Player hits joker!");
            Card playerCard = jokerdeck[0];
            player_hand.Add(playerCard);
            joker_hand.Add(playerCard);
            jokers = jokers + playerCard.name;
            Debug.Log("Player card added to hand: " + playerCard.name);
            RemoveCardFromJokerDeck(0);
            PlayerCard = playerCard;
            PlayerCardInstantiate();
            
        }
        Destroy(JokerHitButton);


    }


    public void AiHit()
    {
        Card aiCard = deck[0];
        aiCard.transform.SetParent(canvasTransform, false); // Set the parent to the Canvas
        ai_hand.Add(aiCard);
        Debug.Log("AI card added to hand: " + aiCard.name);
        RemoveCardFromDeck(0);
        AiCard = aiCard;
        AiCardInstantiate();
    }
    public void AiStand()
    {
        Debug.Log("Ai stands!");
        AiS = true;
    }
    
    public void PlayerBust()
    {
        EndGame.SetActive(true);
        Debug.Log("Player busts!");
        if (AiTotal <= 21)
        {
            WinnerText = "Player Bust, Ai Wins!";
            //Destroy(GameManager.gm.gameObject);
        }
            
    }

    public void AiBust()
    {
        EndGame.SetActive(true);
        Debug.Log("AI busts!");
        if (PlayerTotal <= 21)
        {
            WinnerText = "Ai Bust, Player Wins!";
        }
    }

}
    

