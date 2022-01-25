using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private Image HealthBarImage;
    Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        HealthBarImage = GetComponent<Image>();
        defaultColor = new Color(0.4666667f, 0.3333333f, 0.8666667f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealthBarValue(float value)
    {
        HealthBarImage.fillAmount = value;
        if (HealthBarImage.fillAmount < 0.2f)
        {
            SetHealthBarColor(Color.red);
        }
        else if (HealthBarImage.fillAmount < 0.4f)
        {
            SetHealthBarColor(Color.yellow);
        }
        else
        {
            SetHealthBarColor(defaultColor);
        }
    }

    public float GetHealthBarValue()
    {
        return HealthBarImage.fillAmount;
    }

    public void SetHealthBarColor(Color healthColor)
    {
        HealthBarImage.color = healthColor;
    }
}
