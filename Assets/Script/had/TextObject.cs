using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObject : MonoBehaviour
{
    [SerializeField]
    private CharSprite[] chars;
    [SerializeField]
    private string text;

    private void OnValidate()
    {
        UpdateText(text);
    }

    public void UpdateText(string text)
    {
        this.text = text;

        for (int i = 0; i < chars.Length; i++)
        {
            if (text.Length <= i) break;
            chars[i].SetChar(text[i]);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
