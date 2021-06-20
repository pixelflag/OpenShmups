using UnityEngine;

public interface IGroundWalker
{
    Vector3 position { get; }
    int groundOffset { get; }
    float speedX { get; }
    float speedY { get; }
    void CheckGround(bool isGround, bool isWall);
}
