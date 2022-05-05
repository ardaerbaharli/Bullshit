using System;
using System.Collections.Generic;

[Serializable]
public class Preset
{
    public int NumberOfPlayers;
    public int NumberOfQuestionsPerPlayer;
    public List<Player> Players;
    public int presetID;

    public Preset(int numberOfPlayers, int numberOfQuestionsPerPlayer, List<Player> players)
    {
        NumberOfPlayers = numberOfPlayers;
        NumberOfQuestionsPerPlayer = numberOfQuestionsPerPlayer;
        Players = players;
    }
}