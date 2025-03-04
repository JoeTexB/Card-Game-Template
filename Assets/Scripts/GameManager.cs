using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public int PlayerGames;

    public int AiGames;

    public bool jokerReset;

    public GameObject HitButton;
    public GameObject StandButton;

    private bool isResetting = false;
    private float resetDelay = 5f;
    private float resetCountdown;
    private bool countdownStarted = false;
    private string originalWinnerText;
    private bool hasCountedWin = false;
    private string pendingWinner = "";

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
            DontDestroyOnLoad(gameObject);
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
        jokers = "";
        jokerReset = false;
        EndGame = GameObject.Find("EndGame");
        if (EndGame != null)
        {
            EndGame.SetActive(false);
        }
        
        JokerHitButton = GameObject.Find("JokerHit");
        if (JokerHitButton != null)
        {
            JokerHitButton.SetActive(false);
        }
        
        HitButton = GameObject.Find("Hit");
        StandButton = GameObject.Find("Stand");
        
        Canvas = canvasTransform.position;
        resetCountdown = resetDelay;

        // Initialize and shuffle decks, then deal cards
        InitializeAndDealCards();
    }

    private void InitializeAndDealCards()
    {
        Debug.Log("Initializing and dealing new cards");
        InitializeDeck();
        Shuffle();  // This shuffles both main deck and joker deck
        Deal();
    }

    // Update is called once per frame
    void Update()
    {
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

        // Safer joker button check
        try {
            if (JokerHitButton == null) {
                JokerHitButton = GameObject.Find("JokerHit");
                if (JokerHitButton == null) {
                    // Only log occasionally to avoid spam
                    if (Time.frameCount % 60 == 0) {
                        Debug.LogWarning("JokerHit button not found!");
                    }
                }
            }
            
            if (JokerHitButton != null)
            {
                bool shouldShow = PlayerTotal == AiTotal && 
                                  PlayerTotal >= 17 && 
                                  PlayerTotal <= 21 && 
                                  AiTotal <= 21;
                
                JokerHitButton.SetActive(shouldShow);
            }
        } catch (System.Exception e) {
            Debug.LogError("Error handling joker button: " + e.Message);
        }

        // Only check for win conditions if:
        // 1. Both players have stood, OR
        // 2. Someone has busted
        bool shouldCheckWinConditions = (PlayerS && AiS) || PlayerTotal > 21 || AiTotal > 21;

        // Only check win conditions if the game should be over
        if (shouldCheckWinConditions && !hasCountedWin)
        {
            // First, set EndGame to true if both players have stood
            if (PlayerS && AiS && !EndGame.activeSelf)
            {
                EndGame.SetActive(true);
            }

            // Then check all other win conditions
            if (!string.IsNullOrEmpty(jokers) && (jokers == "JOE" || jokers == "EOJ"))
            {
                EndGame.SetActive(true);
                WinnerText = jokers + " Player Wins!";
                pendingWinner = "Player";
                hasCountedWin = true;
            }
            if (PlayerTotal > AiTotal)
            {
                if (PlayerTotal <= 21)
                {
                    EndGame.SetActive(true);
                    WinnerText = "Player Wins!";
                    pendingWinner = "Player";
                    hasCountedWin = true;
                }
            }
            else if (AiTotal > PlayerTotal)
            {
                if (AiTotal <= 21)
                {
                    EndGame.SetActive(true);
                    WinnerText = "Ai Wins!";
                    pendingWinner = "AI";
                    hasCountedWin = true;
                }
            }
            else if (AiTotal == PlayerTotal)
            {
                if (!EndGame.activeSelf)
                {
                    EndGame.SetActive(true);
                    WinnerText = "Draw, Ai Wins!";
                    originalWinnerText = WinnerText;
                    countdownStarted = true;
                    resetCountdown = resetDelay;
                    pendingWinner = "AI";
                    hasCountedWin = true;
                    
                }

            }
    
        }

        if (PlayerTotal >= 17 && jokerReset == false)
        {
            Debug.Log("JokerCheck 1 Player is above 17");
            if (PlayerTotal == AiTotal || AiTotal > PlayerTotal && AiTotal <= 21)
            {
                Debug.Log("JokerCheck 2 Player meets joker criteria");
                JokerHitButton.SetActive(true);
            }   
        }

        if (EndGame.activeSelf)
        {
            Debug.Log($"Countdown state: started={countdownStarted}, time={resetCountdown}");
            
            if (!countdownStarted)
            {
                countdownStarted = true;
                resetCountdown = resetDelay;
                originalWinnerText = WinnerText;
            }
            
            if (countdownStarted)
            {
                resetCountdown = Mathf.Max(0, resetCountdown - Time.deltaTime);
                
                // Make sure we have a winner text before adding the countdown
                if (string.IsNullOrEmpty(originalWinnerText))
                {
                    // Set a default winner text if it's missing
                    if (PlayerTotal == AiTotal)
                    {
                        originalWinnerText = "Draw, Ai Wins!";
                        pendingWinner = "AI";
                    }
                    else if (AiTotal > PlayerTotal && AiTotal <= 21)
                    {
                        originalWinnerText = "Ai Wins!";
                        pendingWinner = "AI";
                    }
                    else if (PlayerTotal > AiTotal && PlayerTotal <= 21)
                    {
                        originalWinnerText = "Player Wins!";
                        pendingWinner = "Player";
                    }
                    else if (PlayerTotal > 21)
                    {
                        originalWinnerText = "Player Bust, Ai Wins!";
                        pendingWinner = "AI";
                    }
                    else if (AiTotal > 21)
                    {
                        originalWinnerText = "Ai Bust, Player Wins!";
                        pendingWinner = "Player";
                    }
                }
                
                // Now add the countdown to the winner text
                WinnerText = $"{originalWinnerText}\nResetting in: {Mathf.Ceil(resetCountdown)}";
                
                if (resetCountdown <= 0 && !isResetting)
                {
                    isResetting = true;
                    StartCoroutine(ResetGameAfterDelay());
                }
            }
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
        
        // Deactivate Hit and Stand buttons
        GameObject hitButton = GameObject.Find("Hit");
        GameObject standButton = GameObject.Find("Stand");
        
        if (hitButton != null)
        {
            hitButton.SetActive(false);
        }
        
        if (standButton != null)
        {
            standButton.SetActive(false);
        }
    }

    public void PlayerHitJoker()
    {
        // Immediately deactivate the joker button at the start of the method
        if (JokerHitButton != null)
        {
            JokerHitButton.SetActive(false);
            jokerReset = true;
            Debug.Log("Joker button deactivated immediately on click");
        }
        
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
        
        // Check for winning joker combinations immediately
        if (jokers == "JOE" || jokers == "EOJ")
        {
            EndGame.SetActive(true);
            WinnerText = jokers + " Player Wins!";
            originalWinnerText = WinnerText;
            pendingWinner = "Player";
            hasCountedWin = true;
            countdownStarted = true;
            resetCountdown = resetDelay;
        }
        // Always check other win conditions after joker is played
        else
        {
            // Make sure EndGame is active
            EndGame.SetActive(true); //reactivate this if does not end/function
            countdownStarted = true;
            resetCountdown = resetDelay;
            
            if (PlayerTotal == AiTotal)
            {
                WinnerText = "Draw, Ai Wins!";
                originalWinnerText = WinnerText;
                pendingWinner = "AI";
                hasCountedWin = true;
            }
            else if (AiTotal > PlayerTotal && AiTotal <= 21)
            {
                WinnerText = "Ai Wins!";
                originalWinnerText = WinnerText;
                pendingWinner = "AI";
                hasCountedWin = true;
            }
            else if (PlayerTotal > AiTotal && PlayerTotal <= 21)
            {
                WinnerText = "Player Wins!";
                originalWinnerText = WinnerText;
                pendingWinner = "Player";
                hasCountedWin = true;
            }
            else if (PlayerTotal > 21)
            {
                WinnerText = "Player Bust, Ai Wins!";
                originalWinnerText = WinnerText;
                pendingWinner = "AI";
                hasCountedWin = true;
            }
        }
        HitButton.SetActive(false);
        StandButton.SetActive(false);
    }


    public void AiHit()
    {
        Card aiCard = deck[0];
        aiCard.transform.SetParent(canvasTransform, false);
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
        if (!hasCountedWin)
        {
            EndGame.SetActive(true);
            Debug.Log("Player busts!");
            if (AiTotal <= 21)
            {
                WinnerText = "Player Bust, Ai Wins!";
                pendingWinner = "AI";
                hasCountedWin = true;
            }
        }
    }

    public void AiBust()
    {
        if (!hasCountedWin)
        {
            EndGame.SetActive(true);
            Debug.Log("AI busts!");
            if (PlayerTotal <= 21)
            {
                WinnerText = "Ai Bust, Player Wins!";
                pendingWinner = "Player";
                hasCountedWin = true;
            }
        }
    }

    private IEnumerator ResetGameAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        
        // Reset the win counting flag
        hasCountedWin = false;
        
        // Reset jokerReset flag
        jokerReset = false;
        Debug.Log("Reset jokerReset flag to false");
        
        // Destroy all instantiated card clones in the canvas
        foreach (Transform child in canvasTransform)
        {
            // Only destroy card objects, not UI elements
            if (child.GetComponent<Card>() != null)
            {
                Destroy(child.gameObject);
            }
        }
        
        // Find first null slot in each deck
        int deckNullIndex = 0;
        int jokerNullIndex = 0;
        
        for (int i = 0; i < deck.Length; i++)
        {
            if (deck[i] == null)
            {
                deckNullIndex = i;
                break;
            }
        }
        
        for (int i = 0; i < jokerdeck.Length; i++)
        {
            if (jokerdeck[i] == null)
            {
                jokerNullIndex = i;
                break;
            }
        }
        
        // Move joker cards back to joker deck
        foreach (Card card in joker_hand)
        {
            if (jokerNullIndex < jokerdeck.Length)
            {
                jokerdeck[jokerNullIndex] = card;
                jokerNullIndex++;
            }
        }

        // Move player's regular cards back to main deck
        foreach (Card card in player_hand)
        {
            if (!joker_hand.Contains(card))  // Only add to deck if it's not a joker card
            {
                if (deckNullIndex < deck.Length)
                {
                    deck[deckNullIndex] = card;
                    deckNullIndex++;
                }
            }
        }

        // Move AI cards back to main deck
        foreach (Card card in ai_hand)
        {
            if (deckNullIndex < deck.Length)
            {
                deck[deckNullIndex] = card;
                deckNullIndex++;
            }
        }
        
        // Clear all hands
        player_hand.Clear();
        ai_hand.Clear();
        joker_hand.Clear();
        
        // Reset game state
        PlayerTotal = 0;
        AiTotal = 0;
        AiTurn = false;
        PlayerS = false;
        AiS = false;
        PlayerAce = false;
        AiAce = false;
        jokers = "";
        isResetting = false;
        
        // Reset positions
        VAiHand = new Vector2(-300, 600);
        VPlayerHand = new Vector2(-300, 220);
        
        // Reset UI and buttons
        EndGame.SetActive(false);
        
        // Just deactivate the joker button, don't destroy it
        if (JokerHitButton != null)
        {
            JokerHitButton.SetActive(false);
            Debug.Log("Deactivated JokerHit button during reset");
        }
        else
        {
            // Try to find it if we don't have a reference
            JokerHitButton = GameObject.Find("JokerHit");
            if (JokerHitButton != null)
            {
                JokerHitButton.SetActive(false);
                Debug.Log("Found and deactivated JokerHit button during reset");
            }
        }
        
        // Reactivate Hit and Stand buttons using stored references
        if (HitButton != null)
        {
            HitButton.SetActive(true);
        }
        else
        {
            HitButton = GameObject.Find("Hit");
            if (HitButton != null) HitButton.SetActive(true);
        }
        
        if (StandButton != null)
        {
            StandButton.SetActive(true);
        }
        else
        {
            StandButton = GameObject.Find("Stand");
            if (StandButton != null) StandButton.SetActive(true);
        }
        
        WinnerText = "";
        countdownStarted = false;

        // Shuffle and deal
        Shuffle();
        Deal();

        // Count the wins at the end of the round
        if (pendingWinner == "Player")
        {
            PlayerGames++;
            Debug.Log("Player wins counted - Total wins: " + PlayerGames);
        }
        else if (pendingWinner == "AI")
        {
            AiGames++;
            Debug.Log("AI wins counted - Total wins: " + AiGames);
        }
        pendingWinner = "";  // Reset for next game
    }
}
    

