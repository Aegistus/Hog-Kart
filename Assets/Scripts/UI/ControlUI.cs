using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour
{
    [SerializeField] Image panel;
    [SerializeField] bool mouseInput = false;
    [SerializeField] KeyCode inputKey;
    [SerializeField] int mouseButton = 0;
    [SerializeField] Color defaultColor;
    [SerializeField] Color highlightedColor;

    private void Update()
    {
        if (mouseInput)
        {
            if (Input.GetMouseButtonDown(mouseButton))
            {
                panel.color = highlightedColor;
            }
            if (Input.GetMouseButtonUp(mouseButton))
            {
                panel.color = defaultColor;
            }
        }
        else
        {
            if (Input.GetKeyDown(inputKey))
            {
                panel.color = highlightedColor;
            }
            if (Input.GetKeyUp(inputKey))
            {
                panel.color = defaultColor;
            }
        }
    }
}
