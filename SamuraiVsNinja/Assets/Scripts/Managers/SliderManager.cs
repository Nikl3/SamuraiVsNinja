using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderManager : MonoBehaviour {
    
    private Slider mySlider;
    private GameObject thisSlider;
    private float sliderChange;
    private float maxSliderValue;
    private float minSliderValue;
    private float sliderRange;
    private const float SLIDERSTEP = 100.0f; //used to detrime how fine to change value
    //private GameObject inputManager;
    private const string SLIDERMOVE = "Horizontal_J1";
   


    private void Awake() 
    {
        //inputManager = GetComponent<InputManager>().GetHorizontalAxisRaw(1);
        mySlider = GetComponentInParent<Slider>();
        thisSlider = gameObject; //used to deterine when slider has 'focus'
        maxSliderValue = mySlider.maxValue;
        minSliderValue = mySlider.minValue;
        sliderRange = maxSliderValue - minSliderValue;
    }

    private void Update()
    {


        //If slider has 'focus'
        if (thisSlider == EventSystem.current.currentSelectedGameObject)
        {
            sliderChange = Input.GetAxis(axisName: SLIDERMOVE) * sliderRange / SLIDERSTEP;
            float sliderValue = mySlider.value;
            float tempValue = sliderValue + sliderChange;
            if (tempValue <= maxSliderValue && tempValue >= minSliderValue)
            {
                sliderValue = tempValue;
            }
            mySlider.value = sliderValue;
        }
    }
}