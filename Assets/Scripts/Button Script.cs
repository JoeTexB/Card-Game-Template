using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
public class Button : MonoBehaviour
{
  
    public void HitButtonPressed()
    {
        GameManager.gm.PlayerHit();
    }
    public void StandButtonPressed()
    {
        GameManager.gm.PlayerStand();
    }
}
