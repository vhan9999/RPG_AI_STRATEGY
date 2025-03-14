using UnityEngine;

public class GameArgs
{
    public static float GetRewardRatio(Profession profession, RewardType rewardType)
    {
        switch (profession)
        {
            case Profession.Warrior:
                return rewardType == RewardType.Attack ? warriorAttackRatio : warriorHurtRatio;
            case Profession.Berserker:
                return rewardType == RewardType.Attack ? berserkerAttackRatio : berserkerHurtRatio;
            case Profession.Mage:
                return rewardType == RewardType.Attack ? mageAttackRatio : mageHurtRatio;
            case Profession.Archer:
                return rewardType == RewardType.Attack ? archerAttackRatio : archerHurtRatio;
            case Profession.Viking:
                return rewardType == RewardType.Attack ? vikingAttackRatio : vikingHurtRatio;
            case Profession.Tank:
                return rewardType == RewardType.Attack ? tankAttackRatio : tankHurtRatio;
            default:
                return 1f;
        }
    }
    public static bool IsDense = true;
    
    public static float rewardRatio = 1;

    public static float gameCount = 0;

    public static float warriorAttackRatio = 1.2f;
    public static float warriorHurtRatio = 0.2f;

    public static float berserkerAttackRatio = 1.3f;
    public static float berserkerHurtRatio = 0.2f;

    public static float mageAttackRatio = 1f;
    public static float mageHurtRatio = 0.7f;

    public static float archerAttackRatio = 0.8f;
    public static float archerHurtRatio = 0.7f;

    public static float vikingAttackRatio = 1f;
    public static float vikingHurtRatio = 0.3f;

    public static float tankAttackRatio = 1.2f;
    public static float tankHurtRatio = 0.2f;
}


public enum Team
{
    Blue,
    Red
}

public enum Profession
{
    Warrior,
    Mage,
    Berserker,
    Lancer,
    Archer,
    Viking,
    Tank
}

public enum RewardType
{
    Attack,
    Hurt
}