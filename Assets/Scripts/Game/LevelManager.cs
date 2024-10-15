using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{
    public static List<List<CharacterInfo>> levels = new List<List<CharacterInfo>>() 
    {
        // level 1
        new List<CharacterInfo>{new CharacterInfo(Profession.Warrior, new Vector3(6, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(3, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(0, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(-3, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(-6, 1.7f, 5))},

        // level 2
        new List<CharacterInfo>{new CharacterInfo(Profession.Warrior, new Vector3(6, 1.7f, 5)),
                                new CharacterInfo(Profession.Archer, new Vector3(3, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(0, 1.7f, 5)),
                                new CharacterInfo(Profession.Archer, new Vector3(-3, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(-6, 1.7f, 5)) },

        // level 3
        new List<CharacterInfo>{new CharacterInfo(Profession.Berserker, new Vector3(6, 1.7f, 5)),
                                new CharacterInfo(Profession.Mage, new Vector3(3, 1.7f, 5)),
                                new CharacterInfo(Profession.Berserker, new Vector3(0, 1.7f, 5)),
                                new CharacterInfo(Profession.Mage, new Vector3(-3, 1.7f, 5)),
                                new CharacterInfo(Profession.Berserker, new Vector3(-6, 1.7f, 5)) },

        // level 4
        new List<CharacterInfo>{new CharacterInfo(Profession.Tank, new Vector3(8, 1.7f, 5)),
                                new CharacterInfo(Profession.Mage, new Vector3(3, 1.7f, 9)),
                                new CharacterInfo(Profession.Warrior, new Vector3(0, 1.7f, 6)),
                                new CharacterInfo(Profession.Mage, new Vector3(-3, 1.7f, 9)),
                                new CharacterInfo(Profession.Tank, new Vector3(-8, 1.7f, 5)) },

        // level 5
        new List<CharacterInfo>{new CharacterInfo(Profession.Archer, new Vector3(8, 1.7f, 5)),
                                new CharacterInfo(Profession.Tank, new Vector3(3, 1.7f, 9)),
                                new CharacterInfo(Profession.Warrior, new Vector3(0, 1.7f, 6)),
                                new CharacterInfo(Profession.Tank, new Vector3(-3, 1.7f, 9)),
                                new CharacterInfo(Profession.Archer, new Vector3(-8, 1.7f, 5)) },

        // level 6
        new List<CharacterInfo>{new CharacterInfo(Profession.Warrior, new Vector3(7, 1.7f, 9)),
                                new CharacterInfo(Profession.Warrior, new Vector3(3, 1.7f, 7)),
                                new CharacterInfo(Profession.Warrior, new Vector3(1, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(0, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(-1, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(-3, 1.7f, 7)),
                                new CharacterInfo(Profession.Warrior, new Vector3(-7, 1.7f, 9))},


        // level 7
        new List<CharacterInfo>{new CharacterInfo(Profession.Mage, new Vector3(7, 1.7f, 9)),
                                new CharacterInfo(Profession.Berserker, new Vector3(3, 1.7f, 7)),
                                new CharacterInfo(Profession.Warrior, new Vector3(1, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(0, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(-1, 1.7f, 5)),
                                new CharacterInfo(Profession.Berserker, new Vector3(-3, 1.7f, 7)),
                                new CharacterInfo(Profession.Mage, new Vector3(-7, 1.7f, 9))},

        // level 8
        new List<CharacterInfo>{new CharacterInfo(Profession.Archer, new Vector3(8, 1.7f, 9)),
                                new CharacterInfo(Profession.Berserker, new Vector3(6, 1.7f, 7)),
                                new CharacterInfo(Profession.Tank, new Vector3(3, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(0, 1.7f, 5)),
                                new CharacterInfo(Profession.Tank, new Vector3(-3, 1.7f, 5)),
                                new CharacterInfo(Profession.Berserker, new Vector3(-6, 1.7f, 7)),
                                new CharacterInfo(Profession.Archer, new Vector3(-8, 1.7f, 9))},

        // level 9
        new List<CharacterInfo>{new CharacterInfo(Profession.Mage, new Vector3(8, 1.7f, 9)),
                                new CharacterInfo(Profession.Tank, new Vector3(6, 1.7f, 9)),
                                new CharacterInfo(Profession.Berserker, new Vector3(6, 1.7f, 7)),
                                new CharacterInfo(Profession.Warrior, new Vector3(3, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(0, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(-3, 1.7f, 5)),
                                new CharacterInfo(Profession.Berserker, new Vector3(-6, 1.7f, 9)),
                                new CharacterInfo(Profession.Tank, new Vector3(-6, 1.7f, 7)),
                                new CharacterInfo(Profession.Mage, new Vector3(-8, 1.7f, 9))},


        // level 10
        new List<CharacterInfo>{new CharacterInfo(Profession.Mage, new Vector3(8, 1.7f, 9)),
                                new CharacterInfo(Profession.Archer, new Vector3(6, 1.7f, 9)),
                                new CharacterInfo(Profession.Berserker, new Vector3(6, 1.7f, 7)),
                                new CharacterInfo(Profession.Tank, new Vector3(3, 1.7f, 5)),
                                new CharacterInfo(Profession.Warrior, new Vector3(0, 1.7f, 5)),
                                new CharacterInfo(Profession.Tank, new Vector3(-3, 1.7f, 5)),
                                new CharacterInfo(Profession.Berserker, new Vector3(-6, 1.7f, 9)),
                                new CharacterInfo(Profession.Archer, new Vector3(-6, 1.7f, 7)),
                                new CharacterInfo(Profession.Mage, new Vector3(-8, 1.7f, 9))},

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