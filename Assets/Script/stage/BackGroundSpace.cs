using UnityEngine;

public class BackGroundSpace : MonoBehaviour
{
    [SerializeField]
    private BackGroundStar Star = default;

    private BackGroundStar[] stars;
    private int left;
    private int right;

    public void Initialize(ScreenFrame frame)
    {
        left = frame.left;
        right = frame.right;

        int offset = 24;
        int step = 4;
        int h = (frame.height - offset * 2) / step;
        stars = new BackGroundStar[h];

        for (int i = 0; i < h; i++)
        {
            int x = Random.Range(left, right);
            int y = i * step;

            BackGroundStar obj = Instantiate(Star.gameObject, this.transform).GetComponent<BackGroundStar>();
            obj.transform.localPosition = new Vector3(x, y, 0);
            obj.Initialize(new Vector2(Random.Range(0.0f, -1.0f), 0));

            stars[i] = obj;
        }
    }

    public void Execute()
    {
        foreach (BackGroundStar star in stars)
        {
            star.Excute();
            if (star.localX < left - 10) star.localX = right;
        }
    }

    public void Scroll(int scroolSpeed)
    {
        foreach (BackGroundStar star in stars)
        {
            star.x -= scroolSpeed;
        }
    }
}
