using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float existTime;
    public Animator anim;
    public Vector3 moveDir;
    private float timer;
    public ClassAgent agent;
    private bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {

    }
    
    public void Reset()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        timer += deltaTime;
        if (timer > existTime)
        {
            ObjectPool<FireBall>.Instance.Recycle(this);
        }
        if(!isHit)
            transform.Translate(moveDir * Time.deltaTime * speed, Space.World);
    }
    
    private void OnEnable()
    {
        isHit = false;
        anim.SetBool("touch", false);
        anim.SetTrigger("new");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ClassAgent otherAgent))
        {
            if (agent.team != otherAgent.team)
            {
                anim.SetBool("touch", true);
                isHit = true;
                //Debug.Log("great");
                agent.AddReward(1f);
                other.GetComponent<ClassAgent>().TakeDamage(30);
                //other.gameObject.GetComponent<BloodDropletPoolManager>().SpawnBloodDroplets();
            }
            else
            {
                //Debug.Log("Dont'hurt, you are his frend");
                agent.AddReward(-0.3f);
            }
        }
    }

    public void ExplodeDone()
    {
        ObjectPool<FireBall>.Instance.Recycle(this);
    }
}

