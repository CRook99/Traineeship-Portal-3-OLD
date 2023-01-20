using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image icon;
    public Sprite healthyIcon, woundedIcon, dyingIcon;

    void SetHealth(int health)
    {
        slider.value = health;
        UpdateIcon(health);
    }

    void UpdateIcon(int health)
    {
        if (health > 50)
            icon.sprite = healthyIcon;
        else if (health > 20)
            icon.sprite = woundedIcon;
        else
            icon.sprite = dyingIcon;
    }
}
