using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    Slider brightnessSlider;
    Image brightness;
    UIAct uiact;
    Button opCloseButton;
    public bool optionClosed;



    private void Awake()
    {
        brightnessSlider = GetComponentInChildren<Slider>();
        brightness = GameObject.Find("Brightness").GetComponent<Image>();
        opCloseButton = GetComponentInChildren<Button>();
        uiact = new UIAct();
        optionClosed = true;
    }

    private void Start()
    {
        opCloseButton.onClick.AddListener(CloseOption);
        brightnessSlider.onValueChanged.AddListener(ChangeBrightness);  // 밝기조절 옵션 작동방식 변경
    }


    private void OnEnable()
    {
        uiact.UI.Enable();
        uiact.UI.Esc.performed += ESC;
    }

    private void OnDisable()
    {
        uiact.UI.Esc.performed -= ESC;
        uiact.UI.Disable();
    }

    private void ESC(InputAction.CallbackContext _)
    {
        if(optionClosed)
        {
            gameObject.SetActive(false);
        }
    }

    private void CloseOption()
    {
        if (optionClosed)
        {
            gameObject.SetActive(false);
        }
    }

    // 슬라이더 바뀔때만 변경
    private void ChangeBrightness(float arg0)
    {
        brightness.color = new Color(0, 0, 0, brightnessSlider.value);
    }
}
