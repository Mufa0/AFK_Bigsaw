using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderController : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject[] sliderTiles;
    public int currentValue = 0;
    
    public Slider slider;

    public void OnValueChanged()
    {
      for(int i = 0; i < slider.maxValue; ++i)
        {
            if(i < slider.value)
            {
                sliderTiles[i].SetActive(true);
            }
            else
            {
                sliderTiles[i].SetActive(false);
            }
        }
    }
}
