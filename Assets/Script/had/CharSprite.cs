using UnityEngine;

public class CharSprite : MonoBehaviour
{
    [SerializeField]
    private CharLibrary library;
    [SerializeField]
    private SpriteRenderer render;

    public void SetChar(char c)
    {
        render.sprite = library.GetSprite(c);
    }
}
