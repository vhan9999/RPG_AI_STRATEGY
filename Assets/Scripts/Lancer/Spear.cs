using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    private Animator anim;
    public ClassAgent agent;
    private bool IsAttack = false;
    public bool IsAllowedSprint = true;

    [SerializeField]
    private GameObject shockWavePrefab;

    // Shock wave
    [SerializeField] private Transform spawnPoint;
    private int shockWaveFrame = 0;
    private List<ShockWave> shockWaveList = new List<ShockWave>();
    private Vector3[] shockWaveAccelerate = { new Vector3(0.01f, 0.02f, -0.02f), new Vector3(-0.01f
        , 0.02f, -0.02f), new Vector3(0.01f, -0.02f, -0.02f), new Vector3(-0.01f, -0.02f, -0.02f) };

    private void Awake()
    {
        agent = GetComponentInParent<ClassAgent>();
        anim = GetComponent<Animator>();
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
                    shockWave.Acceleration = shockWaveAccelerate[i];
                    shockWaveList.Add(shockWave);
                }
            }

            // Move shockwave
            shockWaveList.ForEach(shockWave => shockWave.Move());

            // Manage shockwave count, keeping it at or below 84
            while (shockWaveList.Count > 84)
            {
                ObjectPool<ShockWave>.Instance.Recycle(shockWaveList[0]);
                shockWaveList.RemoveAt(0);
            }

            if (++shockWaveFrame >= 252)
            {
                shockWaveList.ForEach(shockWave => ObjectPool<ShockWave>.Instance.Recycle(shockWave));
                shockWaveList.Clear();
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
        Debug.Log(IsAllowedSprint);
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

    private void OnTriggerEnter(Collider other)
    {
        if (IsAttack)
        {
            if (other.TryGetComponent(out ClassAgent otherAgent))
            {
                if (agent.team != otherAgent.team)
                {
                    //Debug.Log("great");
                    agent.AddReward(1f);
                    otherAgent.TakeDamage(15);
                }
                else
                {
                    //Debug.Log("Dont'hurt, you are his frend");
                    agent.AddReward(-0.3f);
                }
            }
            //else if (other.TryGetComponent(out Wall wall))
            //{
            //    agent.AddReward(-0.3f);
            //}
        }
    }

    public void ResetThrust()
    {
        IsThrust = false;
    }

    // Method to set the attack state
    public void SetAttackState(int attackState)
    {
        IsAttack = (attackState != 0);
    }
}
