using UnityEngine;

public class Bonus : Item
{
    public override void HitObject(SpriteBase obj)
    {
        base.HitObject(obj);

        switch (itemName)
        {
            case ItemName.Score5000:
                ObjectManager.instance.CreateEffect(EffectName.Score5000, position);
                break;
            case ItemName.OneUp:
                ObjectManager.instance.CreateEffect(EffectName.OneUp, position);
                break;
        }
        FitToPixel();
    }
}
