using System;

[Serializable]
public class HighscoreElement
{
    public string playerName;
    public int points;

    public HighscoreElement(string name, int points)
    {
        this.playerName = name;
        this.points = points;
    }
}
