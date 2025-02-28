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
        
        // Update the winner text first
        WinnerText.text = GameManager.gm.WinnerText;
        
        // Then update the color based on the current text content
        if (WinnerText.text.Contains("Ai Wins") || 
            WinnerText.text.Contains("Player Bust"))
        {
            WinnerText.color = new Color(255, 0, 0, 1);
        }
        else if (WinnerText.text.Contains("Player Wins") || 
                 WinnerText.text.Contains("Ai Bust"))
        {
            WinnerText.color = new Color(0, 201, 0, 1);
        }
        
        // Force the text component to update immediately
        WinnerText.ForceMeshUpdate();
    }
}
            //WinnerText.color = new Color(0,201,0,1); //Green
            //WinnerText.color = new Color(255,0,0,1); //Red