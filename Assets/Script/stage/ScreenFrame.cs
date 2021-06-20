using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFrame
{
    public int width { get; private set; }
    public int height { get; private set; }

    public int right { get; private set; }
    public int left { get; private set; }
    public int top { get; private set; }
    public int bottom { get; private set; }

    public ScreenFrame(int width, int height)
    {
        this.width = width;
        this.height = height;
        right = (int)(width / 2);
        left = (int)(-width / 2);
        top = (int)(height / 2);
        bottom = (int)(-height / 2);
    }

    public void Update(Vector3 position)
    {
        right = (int)(position.x + width / 2);
        left = (int)(position.x - width / 2);
        top =  (int)(position.y + height / 2);
        bottom = (int)(position.y - height / 2);
    }
}
