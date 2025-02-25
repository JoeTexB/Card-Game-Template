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
                    ActionCooldown = 0.5f;
                    AiAction();
                }
            }
    }

    void AiAction()
    {
        if (GameManager.gm.AiTurn == true)
        {
            if (GameManager.gm.AiTotal <= 16 && GameManager.gm.PlayerTotal <= 21)
            {
                GameManager.gm.AiHit();
            }
            if (GameManager.gm.AiTotal >= 17)
            {
                GameManager.gm.AiStand();
            }
            if (GameManager.gm.AiTotal < GameManager.gm.PlayerTotal)
            {
                if (GameManager.gm.PlayerTotal <= 21)
                {
                    GameManager.gm.AiHit();
                }
                if (GameManager.gm.PlayerTotal > 21)
                {
                    GameManager.gm.AiStand();
                }
            }
        }
    }
}
