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

        

    }

    // Update is called once per frame
    void Update()
    {
        PlayerText.text = "Player Score: " + GameManager.gm.PlayerTotal.ToString();
        AiText.text = "AI Score: " + GameManager.gm.AiTotal.ToString();
        WinnerText.text = GameManager.gm.WinnerText;
        
       
    }
}
