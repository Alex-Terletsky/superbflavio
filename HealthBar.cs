using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour //Class to control the status of the health bar
{


    public Slider slider;
    public Gradient gradient;
    public Image fill;
    //Declares used assets for the healthbar


    public void SetMaxHealth(int health) //Method to set the current max health of the player and define the length of the health bar
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health) //Method to set the current health of the player and change the graphic accordingly
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}