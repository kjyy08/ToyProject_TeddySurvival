using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1.2f;
    [SerializeField] protected float runSpeed = 2.1f;
    [SerializeField] protected float animSpeed = 1.2f;
    [SerializeField] protected float animRunSpeed = 1.8f;
    [SerializeField] protected float fieldOfViewAngle = 110f;
    [SerializeField] protected float playerSearchRange = 20f;
    [SerializeField] protected float attackDistance = 0f;
    [SerializeField] protected float attackDelay = 0f;
    [SerializeField] protected float attackDelayTimer = 0f;
    [SerializeField] protected bool isAlive = true;
    [SerializeField] protected string poolItemName;
    [SerializeField] protected string type;

    public int hp = 0;
    public int attackDamage = 0;

    protected bool isPlayerInSight = false;
    protected GameObject player;
    protected Player_Control playerCtrl;
    protected Animator anim;
    protected AudioSource sound;
    protected NavMeshAgent nav;
    protected CapsuleCollider col;
    protected new Rigidbody rigidbody;
    protected enum ENEMY_TYPE { NONE, WALKER, DASH, NORMAL, GHOST, JUMPER, HELLEPHANT }
    protected ENEMY_TYPE eType = ENEMY_TYPE.NONE;

    public virtual void FootStep(string name)
    {
        //sound.clip = SoundManager.instance.GetAudioClip(name);
        //sound.Play();
    }

    public virtual void Growl(string name)
    {
        sound.PlayOneShot(SoundManager.Instance.GetAudioClip(name));
    }

    protected void SetBaseStat()
    {
        //hp += (EnemyInfo.Instance.enemyInfos[type].hp - hp);
        //attackDamage = EnemyInfo.Instance.enemyInfos[type].damage;
        //moveSpeed = EnemyInfo.Instance.enemyInfos[type].moveSpeed;
        //runSpeed = EnemyInfo.Instance.enemyInfos[type].runSpeed;

        hp += DBManager.Instance.m_monster.monsterStatDictionary[type].hp;
        attackDamage = DBManager.Instance.m_monster.monsterStatDictionary[type].damage;
        moveSpeed = DBManager.Instance.m_monster.monsterStatDictionary[type].moveSpeed;
        runSpeed = DBManager.Instance.m_monster.monsterStatDictionary[type].runSpeed;
    }


    protected virtual IEnumerator Init()
    {
        yield return null;
    }

    protected virtual IEnumerator FSM_Main()
    {
        yield return null;
    }

    protected virtual IEnumerator Patrol()
    {
        yield return null;
    } 

    protected virtual IEnumerator Walk()
    {
        yield return null;
    }

    protected virtual IEnumerator Listen()
    {
        yield return null;
    }

    protected virtual IEnumerator Chase()
    {
        yield return null;
    }

    protected virtual IEnumerator Attack()
    {
        yield return null;
    }

    protected virtual IEnumerator Dead()
    {
        yield return null;
    }

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCtrl = player.GetComponent<Player_Control>();
        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>();
        col = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        
    }
}
