using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IBehaviorTreeInterface, ITeamInterface, ISpawnInterface
{
    [SerializeField] int teamId = 2;

    [SerializeField] Reward killingReward;

    private Animator anim;
    private HealthComponent healthComponent;
    private PerceptionComponent perceptionComponent;
    private BehaviorTree behaviorTree;
    private MovementComponent movementComponent;

    private Vector3 prevPosition;

    public Animator Anim
    {
        get => this.anim;
        private set => this.anim = value;
    }

    public int GetTeamID() => teamId;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        healthComponent = GetComponent<HealthComponent>();
        behaviorTree = GetComponent<BehaviorTree>();
        movementComponent = GetComponent<MovementComponent>();
        perceptionComponent = GetComponent<PerceptionComponent>();

        perceptionComponent.onPerceptionTargetChanged += PerceptionComponent_onPerceptionTargetChanged;
    }

    protected virtual void Start()
    {
        

        if(healthComponent != null)
        {
            healthComponent.onHealthEmpty += HealthComponent_onHealthEmpty;
            healthComponent.onTakeDamage += HealthComponent_onTakeDamage;
        }

        prevPosition = transform.position;
    }

    private void Update()
    {
        CalculateSpeed();
    }

    private void CalculateSpeed()
    {
        if (movementComponent == null) return;

        Vector3 deltaPos = transform.position - prevPosition;
        float speed = deltaPos.magnitude / Time.deltaTime;
        anim.SetFloat(StringCollector.speedAnim, speed);

        prevPosition = transform.position;
    }

    private void PerceptionComponent_onPerceptionTargetChanged(GameObject target, bool sensed)
    {
        if (sensed)
        {
            behaviorTree.BlackBoard.SetOrAddData(StringCollector.targetString, target);
        }
        else
        {
            behaviorTree.BlackBoard.SetOrAddData(StringCollector.lastSeenPosString, target.transform.position);
            behaviorTree.BlackBoard.RemoveBlackboardData(StringCollector.targetString);
        }
    }

    private void HealthComponent_onTakeDamage(float currentHealth, float delta, float maxHealth, GameObject instigator)
    {
        
    }

    private void HealthComponent_onHealthEmpty(GameObject killer)
    {
        TriggerDeathAnimation();

        IRewardListener[] rewardListeners = killer.GetComponents<IRewardListener>();
        foreach(var listner in rewardListeners)
        {
            listner.Reward(killingReward);
        }
    }

    private void TriggerDeathAnimation()
    {
        if(anim != null)
        {
            anim.SetTrigger(StringCollector.deadAnim);
        }
    }

    public void OnDeathAnimationFinished()
    {
        Dead();
        Destroy(this.gameObject);
    }

    protected virtual void Dead()
    {

    }

    public void RotateToward(GameObject target, bool verticalAim = false)
    {
        Vector3 aimDir = target.transform.position - transform.position;
        aimDir.y = verticalAim ? aimDir.y : 0f;
        aimDir = aimDir.normalized;

        movementComponent.RotateToward(aimDir);
    }

    public virtual void AttackTarget(GameObject target)
    {
        
    }

    public void SpawnBy(GameObject spawner)
    {
        BehaviorTree spawnerBehaviorTree = spawner.GetComponent<BehaviorTree>();

        if(spawnerBehaviorTree != null && 
            spawnerBehaviorTree.BlackBoard.GetBlackboardData<GameObject>(
                            StringCollector.targetString, out GameObject target))
        {
            PerceptionStimulus stimulus = target.GetComponent<PerceptionStimulus>();

            if(perceptionComponent != null && stimulus != null)
            {
                perceptionComponent.AssignPerceiveStimulus(stimulus);
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if(target != null)
    //    {
    //        Gizmos.DrawWireSphere(target.transform.position + Vector3.up, 1f);

    //        Gizmos.DrawLine(transform.position + Vector3.up, target.transform.position + Vector3.up);
    //    }
    //}
}
