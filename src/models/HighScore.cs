

using System.Collections.Generic;
[System.Serializable]
public class HighScore
{
    public string name;
    public int score;

    public HighScore(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}

[System.Serializable]
public class HighScoreWrapper
{
    public List<HighScore> l;
}