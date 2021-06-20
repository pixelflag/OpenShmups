using UnityEngine;

public struct Box
{
    public Box(Vector2 position, int extendsX, int extendsY)
    {
        _position = position;
        _extendsX = extendsX;
        _extendsY = extendsY;
    }

    public float top { get { return _position.y + _extendsY; } }
    public float right { get { return _position.x + _extendsX; } }
    public float bottom { get { return _position.y - _extendsY; } }
    public float left { get { return _position.x - _extendsX; } }

    public Vector2 topLeft
    {
        get { return new Vector2(left, top); }
    }
    public Vector2 topRight
    {
        get { return new Vector2(right, top); }
    }
    public Vector2 bottomLeft
    {
        get { return new Vector2(left, bottom); }
    }
    public Vector2 bottomRight
    {
        get { return new Vector2(right, bottom); }
    }

    private Vector2 _position;
    public Vector2 position
    {
        get { return _position; }
        set { _position = value; }
    }
    public float x
    {
        get { return _position.x; }
        set { _position.x = value; }
    }
    public float y
    {
        get { return _position.y; }
        set { _position.y = value; }
    }

    private int _extendsX;
    public int extendsX
    {
        get { return _extendsX; }
        set { _extendsX = value; }
    }

    private int _extendsY;
    public int extendsY
    {
        get { return _extendsY; }
        set { _extendsY = value; }
    }
}
