using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBase : PixelObject
{
    [SerializeField]
    protected Sprite[] sprites;

    private GameObject baseSprite;
    protected SpriteRenderer render;

    public bool isDead { get; set; }

    protected Box hitBox;
    public Box GetHitBox()
    {
        hitBox.position = position;
        return hitBox;
    }

    public override void Initialize()
    {
        base.Initialize();
        baseSprite = transform.Find("BaseSprite").gameObject;
        render = baseSprite.GetComponent<SpriteRenderer>();

        hitBox = new Box(position, 1, 1);
    }

    public override void ResetObject()
    {
        base.ResetObject();
        isDead = false;
    }

    public void FitToPixel()
    {
        Vector3 pos = position;
        pos.x = (int)pos.x;
        pos.y = (int)pos.y;
        baseSprite.transform.position = pos;
    }
}
