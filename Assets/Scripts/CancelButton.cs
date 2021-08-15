using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelButton : MonoBehaviour
{
    public Material greyScale;
    public Material basicMaterial;
    private Button btn;
    private Image img;

    private void Start()
    {
        btn = GetComponentInChildren<Button>();
        img = btn.gameObject.GetComponent<Image>();
    }

    public void EnableInteraction()
    {
        btn.interactable = true;
        img.material = basicMaterial;

    }

    public void DisableInteraction()
    {
        btn.interactable = false;
        img.material = greyScale;
    }
}
