using UnityEngine;

public class BackGroundStar : PixelObject
{
    public Sprite[] sprites;
    private Vector2 speed;

    public void Initialize(Vector2 speed)
    {
        base.Initialize();
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
        this.speed = speed;
        z = 100;
    }

    public void Excute()
    {
        x += speed.x;
    }
}
