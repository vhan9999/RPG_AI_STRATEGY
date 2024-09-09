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
            default:
                return 1f;
        }
    }
    public static bool IsDense = true;
    public static float attack = 2f;
    public static float hurt = 0.5f;

    public static float warriorAttackRatio = 2f;
    public static float warriorHurtRatio = 0.1f;

    public static float berserkerAttackRatio = 1.8f;
    public static float berserkerHurtRatio = 0.1f;

    public static float mageAttackRatio = 1.2f;
    public static float mageHurtRatio = 0.3f;

    public static float archerAttackRatio = 1.2f;
    public static float archerHurtRatio = 0.3f;

    public static float vikingAttackRatio = 1.8f;
    public static float vikingHurtRatio = 0.1f;
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