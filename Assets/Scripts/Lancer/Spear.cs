using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon
{
    public bool IsAllowedSprint = true;

    // Shock wave
    [SerializeField] private GameObject shockWavePrefab;
    [SerializeField] private Transform spawnPoint;
    private int shockWaveFrame = 0;
    private Queue<ShockWave> shockWaveQueue = new Queue<ShockWave>();
    private Vector3[] shockWaveInitState = { new Vector2(-1, 1), new Vector2(1, 1), 
        new Vector2(-1, -1), new Vector2(1, -1)};

    protected override void Awake()
    {
        base.Awake();
        ObjectPool<ShockWave>.Instance.InitPool(shockWavePrefab, 20);
    }

    private void OnEnable()
    {
        IsSprint = false;
        IsAllowedSprint = true;
    }


    private void Update()
    {

    }

    private void FixedUpdate()
    {
        // Check if sprinting is active
        if (IsSprint && shockWaveFrame < 252)
        {
            int frame = shockWaveFrame % 20;
            if (frame < 5)
            {
                for (int i = 0; i < 4; i++)
                {
                    ShockWave shockWave = ObjectPool<ShockWave>.Instance.Spawn(spawnPoint.position, Quaternion.identity, transform);
                    shockWave.SetInitState(shockWaveInitState[i]);
                    shockWaveQueue.Enqueue(shockWave);
                }
            }

            // Move shockwave
            foreach (ShockWave shockWave in shockWaveQueue)
            {
                shockWave.Move();
            }

            // Manage shockwave count, keeping it at or below 84
            while (shockWaveQueue.Count > 84)
            {
                ShockWave shockWave = shockWaveQueue.Dequeue();
                ObjectPool<ShockWave>.Instance.Recycle(shockWave);
            }

            // Check if the sprint state should end
            if (++shockWaveFrame >= 252)
            {
                foreach (ShockWave shockWave in shockWaveQueue)
                {
                    ObjectPool<ShockWave>.Instance.Recycle(shockWave);
                }
                shockWaveQueue.Clear();
                IsSprint = false;
                Invoke("EnableSprint", 15f);
            }
        }
    }

    public bool IsThrust
    {
        get => anim.GetBool("isThrust");
        set => anim.SetBool("isThrust", value);
    }

    public bool IsSprint
    {
        get => anim.GetBool("isSprint");
        set => anim.SetBool("isSprint", value);
    }

    // Attack enemy
    public void Thrust()
    {
        if (!IsThrust)
        {
            agent.AddReward(-0.2f);
            IsThrust = true;
        }
    }

    // Use sprint to attack the enemies
    public void Sprint()
    {
        if (IsAllowedSprint)
        {
            agent.AddReward(-0.3f);
            IsThrust = false;
            IsSprint = true;
            IsAllowedSprint = false;
            shockWaveFrame = 0;
        }
    }


    private void EnableSprint()
    {
        IsAllowedSprint = true;
    }

    public void ResetThrust()
    {
        IsThrust = false;
    }
}
