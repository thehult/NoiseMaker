using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderWrap : MonoBehaviour
{

    Text _label;
    Slider _slider;
    InputField _input;

    public string label = "Control";
    public float value = 1f;
    public float minValue = 0f;
    public float maxValue = 1f;
    public bool integer = false;

    public ValueChanged valueChanged;

    // Start is called before the first frame update
    void Start()
    {
        _label = GetComponentInChildren<Text>();
        _slider = GetComponentInChildren<Slider>();
        _input = GetComponentInChildren<InputField>();

        _label.text = label;
        _slider.minValue = minValue;
        _slider.maxValue = maxValue;
        _slider.value = value;
        _slider.wholeNumbers = integer;
        _input.text = value.ToString();
        if(integer)
            _input.contentType = InputField.ContentType.IntegerNumber;
    }

    bool ignoreNextChange = false;
    public void SliderChange(float value)
    {
        if (ignoreNextChange)
        {
            ignoreNextChange = false;
            return;
        }
        ignoreNextChange = true;
        this.value = value;
        _input.text = value.ToString();
        valueChanged?.Invoke(this.value);
    }

    public void InputChange(string text)
    {
        if (ignoreNextChange)
        {
            ignoreNextChange = false;
            return;
        }
        
        text = text.Replace(',', '.');
        if(float.TryParse(text, out value))
        {
            ignoreNextChange = true;
            this._slider.value = value;
            valueChanged?.Invoke(this.value);
        }
    }

    [System.Serializable]
    public class ValueChanged : UnityEvent<float> { }
}
