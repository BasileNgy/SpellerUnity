using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class HealthBar : MonoBehaviour
{
    public Sprite FilledHeart;
    public Sprite EmptyHeart;

    public Image heart1;
    public Image heart2;
    public Image heart3;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setFilledHearts(int health)
    {
        if (health >= 1)
            heart1.sprite = FilledHeart;
        else
            heart1.sprite = EmptyHeart;

        if (health >= 2)
            heart2.sprite = FilledHeart;
        else
            heart2.sprite = EmptyHeart;

        if (health == 3)
            heart3.sprite = FilledHeart;
        else
            heart3.sprite = EmptyHeart;
    }
}
