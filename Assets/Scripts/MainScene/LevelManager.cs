using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelManager 
{
    private int _level;
    private bool _levelStatus;
    private bool _star1;
    private bool _star2;
    private bool _star3;

    public int Level { get => _level; set => _level = value; }
    public bool LevelStatus { get => _levelStatus; set => _levelStatus = value; }
    public bool Star1 { get => _star1; set => _star1 = value; }
    public bool Star2 { get => _star2; set => _star2 = value; }
    public bool Star3 { get => _star3; set => _star3 = value; }

    public LevelManager(int _level,bool _levelStatus, bool _star1, bool _star2, bool _star3)
    {
        this.Level = _level;
        this.LevelStatus = _levelStatus;
        this.Star1 = _star1;
        this.Star2 = _star2;
        this.Star3 = _star3;
    }

}
