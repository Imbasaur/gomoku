﻿using Gomoku.DAL.Abstract;

namespace Gomoku.DAL.Entities;
public sealed class PlayerWaiting : IEntity<int>
{
    public int Id { get; set; }
    public string PlayerName { get; set; }
    //TODO: waiting settings in future? depends if this will be saved in profile or per game? prolly defaults in profile...

    public PlayerWaiting(string playerName) => PlayerName = playerName;

    public PlayerWaiting()
    { }
}
