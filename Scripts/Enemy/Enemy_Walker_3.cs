using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Walker_3 : Enemy
{
    private enum eState { Patrol, Chase, Attack, Dead }
    [SerializeField] private eState state = eState.Patrol;
    private Vector3 lastPlayerSightingPosition;

    public float pushPoolDelay = 0f;
    public float elapsedTime = 0f;

    protected override IEnumerator Init()
    {
        yield return new WaitForEndOfFrame();

        SetBaseStat();
        StartCoroutine(FSM_Main());
        StartCoroutine(CheckValues());
    }

    protected override IEnumerator FSM_Main()
    {
        while (!PlayerInfo.Instance.playerBaseInfo.isDead)
        {
            yield return StartCoroutine(state.ToString());
        }
    }
    
    protected override IEnumerator Patrol()
    {
        nav.SetDestination(player.transform.position);
        state = eState.Chase;

        yield return null;
    }
    
    protected override IEnumerator Chase()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= attackDistance)
        {
            state = eState.Attack;
        }

        else
        {
            lastPlayerSightingPosition = (player.transform.position - transform.position).normalized;
            lastPlayerSightingPosition.y = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lastPlayerSightingPosition), 0.1f);   // Quaternion.LookRotation(rotDir);
            nav.SetDestination(player.transform.position);
        }

        yield return null;
    }

    protected override IEnumerator Attack()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > attackDistance)
        {
            attackDelayTimer = 0f;
            state = eState.Chase;
        }

        else
        {
            if (attackDelayTimer <= 0f)
            {
                attackDelayTimer = attackDelay;
                playerCtrl.Hit();
            }

            else
            {
                attackDelayTimer -= Time.deltaTime;
            }
        }

        yield return null;
    }

    protected override IEnumerator Dead()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= pushPoolDelay)
        {
            isAlive = false;
            elapsedTime = 0f;
            ObjectPool.Instance.PushToPool(poolItemName, gameObject);
        }

        yield return null;
    }

    protected IEnumerator CheckValues()
    {
        while (isAlive)
        {
            if (hp <= 0)
            {
                state = eState.Dead;
                nav.isStopped = true;
                nav.enabled = false;
                anim.SetBool("isDead", true);
                anim.SetFloat("velocity", 0f);
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
                col.enabled = false;
                EnemySpawner.currentEnemyNum -= 1;
                break;
            }

            switch (state)
            {
                case eState.Patrol:
                    nav.isStopped = false;
                    nav.speed = moveSpeed;
                    anim.speed = animSpeed;
                    break;

                case eState.Chase:
                    nav.isStopped = false;
                    nav.speed = moveSpeed;
                    anim.speed = animSpeed;
                    break;

                case eState.Attack:
                    nav.isStopped = true;
                    nav.velocity = Vector3.zero;
                    break;

                case eState.Dead:
                    
                    break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        isAlive = true;
        attackDelayTimer = 0f;
        eType = ENEMY_TYPE.WALKER;
        state = eState.Patrol;
        nav.enabled = true;
        nav.isStopped = false;
        nav.speed = moveSpeed;
        anim.speed = animSpeed;
        anim.SetBool("isDead", false);
        anim.SetFloat("velocity", 0f);
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        col.enabled = true;
        StartCoroutine(Init());
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            hp -= DBManager.Instance.m_weapon.weaponStatDictionary[PlayerInfo.Instance.gtype.ToString()].damage;

            GameObject hitEffect = ObjectPool.Instance.PopFromPool("EnemyHitEffect");
            hitEffect.transform.position = other.transform.position;
            hitEffect.transform.rotation = other.transform.rotation;
            hitEffect.SetActive(true);
        }
    }
    
}
