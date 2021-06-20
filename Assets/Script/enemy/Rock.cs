using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Enemy
{
    private Vector2 force = new Vector2();
    private float gravity = -0.2f;

    public override void Initialize(EnemySpawnData data)
    {
        disableAutoShot = true;
        base.Initialize(data);
        render.sprite = sprites[Random.Range(0,2)];
        hitBox = new Box(position, 4, 4);
    }

    public override void Execute()
    {
        base.Execute();

        x += force.x;
        y += force.y;

        force.y += gravity;

        FitToPixel();
    }

    public void AddForce(Vector2 force)
    {
        this.force = force;
    }
}
