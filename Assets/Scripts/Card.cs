using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Card_data data;

    public string card_name;
    public string description;
    public int health;
    public int cost;
    public int damage;
    public Sprite sprite;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI damageText;
    public Image spriteImage;
        

    // Start is called before the first frame update
    void Start()
    {
        card_name = "      " + data.card_name;
        description = data.description;
        health = data.cost;
        cost = data.cost;
        damage = data.cost;
        sprite = data.sprite;
        nameText.text = card_name;
        descriptionText.text = description;
        healthText.text = health.ToString();
        costText.text = cost.ToString();
        damageText.text = damage.ToString();
        spriteImage.sprite = sprite;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
