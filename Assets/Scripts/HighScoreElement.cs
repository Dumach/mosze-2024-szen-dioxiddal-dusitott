using System;

[Serializable]
public class HighscoreElement
{
    public string playerName;
    public int points;

    public HighscoreElement(string name, int points)
    {
        playerName = name;
        this.points = points;
    }
}
