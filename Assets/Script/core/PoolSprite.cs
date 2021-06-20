using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSprite : SpriteBase
{
    public void Spawn(Vector3 position)
    {
        isDead = false;
        this.position = position;
    }

    public void DestroyObject()
    {
        isDead = true;
        y = 10000;
    }
}
