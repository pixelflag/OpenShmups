using System.Text;
using UnityEngine;

public class HadManager : MonoBehaviour
{
    public static HadManager instance;
    private StringBuilder sb = new StringBuilder();

    [SerializeField]
    private TextObject LEFTtext;
    [SerializeField]
    private TextObject P1ScoreText;
    [SerializeField]
    private TextObject HiScoreText;
    [SerializeField]
    private TextObject GameOverText;

    [SerializeField]
    private PowerGauge powerGauge = default;

    public int p1Left { get; set; }
    private int p1Score = 0;
    private int hiScore = 50000;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameOverText.Hide();
    }

    public void PlusLeft()
    {
        p1Left++;
        LEFTtext.UpdateText((p1Left > 9 ? 9 : p1Left).ToString());
    }

    public void MinusLeft()
    {
        p1Left--;
        LEFTtext.UpdateText((p1Left < 0 ? 0 : p1Left).ToString());
    }

    public void AddScore(int score)
    {
        p1Score += score;
        p1Score = p1Score > 9999900 ? 9999900 : p1Score;
        P1ScoreText.UpdateText(ScoreToString(p1Score));

        if (p1Score > hiScore)
        {
            hiScore = p1Score;
            HiScoreText.UpdateText(ScoreToString(hiScore));
        }
    }

    public void ResetAll()
    {
        p1Left = 3;
        p1Score = 0;
        GameOverText.Hide();
        powerGauge.Show();

        LEFTtext.UpdateText(p1Left.ToString());
        P1ScoreText.UpdateText(ScoreToString(p1Score));
        HiScoreText.UpdateText(ScoreToString(hiScore));
    }

    public void ShowGameOver()
    {
        GameOverText.Show();
        powerGauge.Hide();
    }

    public void UpdateGauge(Player player)
    {
        powerGauge.UpdateGauge(player);
    }

    private string ScoreToString(int score)
    {
        sb.Clear();
        string s = score.ToString();

        for(int i=0; i < 7 - s.Length; i++)
        {
            sb.Append("0");
        }
        sb.Append(s);

        return sb.ToString();
    }
}
