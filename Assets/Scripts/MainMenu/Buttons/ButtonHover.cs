using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Reference to the Button
    public TMP_Text text;
    public Color onEnterColor;

    private Color onExitColor;

    void Start()
    {
        onExitColor = text.color;
    }
    // Called when the pointer enters the button area
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Entered the Button!");
        // You can change the button's color or perform other actions here
        text.color = onEnterColor; // For example, change button color to green
    }

    // Called when the pointer exits the button area
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exited the Button!");
        // Reset the button's color or perform other actions here
        text.color = onExitColor; // Reset color back to white
    }
}