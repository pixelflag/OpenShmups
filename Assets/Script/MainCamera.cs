using UnityEngine;

public class MainCamera : PixelObject
{
    [SerializeField]
    private BackGroundSpace space;

    public ScreenFrame screenFrame { get; private set; }

    public bool isStopScroll { get; set; }
    private float scrollSpeed = 0.6f;
    private float scrollCount;

    public override void Initialize()
    {
        base.Initialize();
        screenFrame = new ScreenFrame(256,224);

        space.Initialize(screenFrame);
    }

    public void CameraUpdate()
    {
        if (!isStopScroll)
        {
            scrollCount += scrollSpeed;
            if (1 < scrollCount)
            {
                scrollCount -= 1;
                Scroll(1);
            }
        }

        space.Execute();
    }

    public void ResetPosition(int x)
    {
        this.x = x;
        this.y = 0;
        screenFrame.Update(position);
    }

    public void Scroll(int speed)
    {
        x += speed;
        space.Scroll(speed);
        screenFrame.Update(position);
    }
}
