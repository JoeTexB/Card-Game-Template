using UnityEngine;

public class AIScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private float ActionCooldown;
    void Start()
    {
        ActionCooldown = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
       if (GameManager.gm.AiTurn == true)
            {
                ActionCooldown -= Time.deltaTime;
                if (ActionCooldown <= 0)
                {
                    ActionCooldown = 1f;
                    AiAction();
                    
                }
            }
    }

    void AiAction()
    {
        if (GameManager.gm.AiTurn == true)
        {
            Debug.Log($"AI deciding action - AI:{GameManager.gm.AiTotal} P:{GameManager.gm.PlayerTotal} PS:{GameManager.gm.PlayerS}");
            
            // First check if player has busted
            if (GameManager.gm.PlayerTotal > 21)
            {
                Debug.Log("AI stands - player busted");
                GameManager.gm.AiStand();  // AI should stand if player has busted
            }
            // AI should hit if player has higher score (and AI is below 21)
            else if (GameManager.gm.AiTotal < GameManager.gm.PlayerTotal && GameManager.gm.PlayerTotal <= 21 && GameManager.gm.AiTotal < 21)
            {
                Debug.Log("AI hits - player has higher score");
                GameManager.gm.AiHit();
            }
            // Stand if AI has 17 or more
            else if (GameManager.gm.AiTotal >= 17)
            {
                Debug.Log("AI stands - has 17 or more");
                GameManager.gm.AiStand();
            }
            // Otherwise use normal AI logic for hitting
            else if (GameManager.gm.AiTotal <= 16)
            {
                Debug.Log("AI hits - total <= 16");
                GameManager.gm.AiHit();
            }
            else
            {
                Debug.Log("AI stands - default case");
                GameManager.gm.AiStand();
            }
        }
    }
}
