using UnityEngine;
using TMPro;
public class PlayerCardTotal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
    
    
    public TextMeshProUGUI PlayerText;
    public TextMeshProUGUI AiText;
    public TextMeshProUGUI WinnerText;



    void Start()
    {

        WinnerText.color = new Color(255, 0, 0, 1);

    }

    // Update is called once per frame
    void Update()
    {
        PlayerText.text = "Player Score: " + GameManager.gm.PlayerTotal.ToString();
        AiText.text = "AI Score: " + GameManager.gm.AiTotal.ToString();
        
        
        if (GameManager.gm.WinnerText == "Player Bust, Ai Wins!")
        {
            WinnerText.color = new Color(255,0,0,1);
        }
        if (GameManager.gm.WinnerText == "Draw, Ai Wins!")
        {
            WinnerText.color = new Color(255,0,0,1);           
        }
        if (GameManager.gm.WinnerText == "Ai Wins!")
        {
            WinnerText.color = new Color(255,0,0,1);        
        }
        
        if (GameManager.gm.WinnerText == "Ai Bust, Player Wins!")
        {
            WinnerText.color = new Color(0,201,0,1);           
        }
        if (GameManager.gm.WinnerText == "Player Wins!" || GameManager.gm.WinnerText == "EOJ Player Wins!" || GameManager.gm.WinnerText == "JOE Player Wins!") 
        {
            WinnerText.color = new Color(0,201,0,1);
        }
        
        WinnerText.text = GameManager.gm.WinnerText;
        
        

       
    }
}
            //WinnerText.color = new Color(0,201,0,1); //Green
            //WinnerText.color = new Color(255,0,0,1); //Red