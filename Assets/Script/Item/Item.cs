using UnityEngine;

public class Item : SpriteBase
{
    [SerializeField]
    public ItemName itemName;

    public virtual void Initialize(ItemName itemName)
    {
        base.Initialize();

        this.itemName = itemName;
        hitBox = new Box(position, 12, 12);
    }

    public virtual void HitObject(SpriteBase obj)
    {
        isDead = true;
    }
}
