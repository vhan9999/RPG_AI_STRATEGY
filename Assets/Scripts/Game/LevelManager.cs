using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{
    public static List<List<CharacterInfo>> levels = new List<List<CharacterInfo>>() 
    {
        new List<CharacterInfo>{new CharacterInfo(Profession.Warrior, new Vector3(6, 1.3f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(3, 1.3f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(0, 1.3f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(-3, 1.3f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(-6, 1.3f, 5))},
        new List<CharacterInfo>{ },
    };
}

public class CharacterInfo
{
    public Profession Profession { get; set; }
    public Vector3 StartingPos { get; set; }

    public CharacterInfo(Profession profession, Vector3 startingPos)
    {
        Profession = profession;
        StartingPos = startingPos;
    }
}