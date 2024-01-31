using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player_Control : MonoBehaviour
{
    public AudioSource sound;
    public Animator animator;
    public ParticleSystem effect;

    private float moveSpeed = 0f;
    private float h;
    private float v;
    private Vector3 moveDir;
    public bool is_fire = false;
    public bool is_reload = false;
    private new Rigidbody rigidbody;
    private CapsuleCollider col;
    public UnityEngine.UI.Image hitImg;

    [SerializeField] private PlayerInfo.GunInfo[] _gunInfo;
    public Dictionary<string, PlayerInfo.GunInfo> gunInfo = new Dictionary<string, PlayerInfo.GunInfo>();

    public float attackTimer = 0.0f;
    public float reloadTimer = 0.0f;
    public float weaponChangeDelay = 0.0f;
    public float weaponChangeTimer = 0.0f;

    public enum GunType { AK, Sniper, Shotgun, };

    void PlaySoundClip(string name)
    {
        sound.PlayOneShot(SoundManager.Instance.GetAudioClip(name));
    }

    private void Init()
    {
        foreach (PlayerInfo.GunInfo i in _gunInfo)
        {
            gunInfo[i.gunType] = i;
        }
    }
    
    IEnumerator Reload(string type)
    {
        is_reload = true;
        is_fire = false;
        reloadTimer = 0f;
        PlaySoundClip("Reload");

        while (reloadTimer < gunInfo[type].reloadDelay)
        {
            reloadTimer += Time.deltaTime;

            yield return null;
        }

        gunInfo[type].remainingMagazines = gunInfo[type].maxMagazines;
        attackTimer = gunInfo[type].attackDelay;
        reloadTimer = 0f;
        is_reload = false;
    }

    void Awake()
    {
        Init();
        rigidbody = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        sound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        moveSpeed = PlayerInfo.Instance.playerBaseInfo.moveSpeed;
        PlayerInfo.Instance.gtype = GunType.AK;
        gunInfo[PlayerInfo.Instance.gtype.ToString()].remainingMagazines = gunInfo[PlayerInfo.Instance.gtype.ToString()].maxMagazines;
    }

    void Update()
    {
        if (PlayerInfo.Instance.playerBaseInfo.hp <= 0f)
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
            col.enabled = false;
            PlayerInfo.Instance.playerBaseInfo.isDead = true;
        }

        if (!PlayerInfo.Instance.playerBaseInfo.isDead)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            
            if (Input.GetKey(KeyCode.R))
            {
                string type = PlayerInfo.Instance.gtype.ToString();

                if (!is_reload)
                    StartCoroutine(Reload(type));
            }

            if (!is_reload)
                is_fire = Input.GetKey(KeyCode.Space);

            if (Input.GetKey(KeyCode.Alpha1))
            {
                ChangeWeapon(PlayerInfo.Instance.gtype.ToString(), GunType.AK.ToString());
            }

            else if (Input.GetKey(KeyCode.Alpha2))
            {
                ChangeWeapon(PlayerInfo.Instance.gtype.ToString(), GunType.Shotgun.ToString());
            }

            else if (Input.GetKey(KeyCode.Alpha3))
            {
                ChangeWeapon(PlayerInfo.Instance.gtype.ToString(), GunType.Sniper.ToString());
            }

            if (h != 0 || v != 0)
            {
                animator.SetBool("IsWalking", true);
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }
        }
        else
        {
            animator.SetBool("IsDead", true);
        }
    }

    void FixedUpdate()
    {
        if (!PlayerInfo.Instance.playerBaseInfo.isDead)
        {
            moveDir = new Vector3(h, 0f, v);

            transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.World);

            Vector3 cameraPos = Camera.main.transform.position;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                cameraPos.y));

            Vector3 rotDir = (mousePosition - transform.position).normalized;
            rotDir.y = 0f;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotDir), 0.2f);   // Quaternion.LookRotation(rotDir);

            string type = PlayerInfo.Instance.gtype.ToString();

            if (gunInfo[PlayerInfo.Instance.gtype.ToString()].remainingMagazines <= 0)
            {
                if (!is_reload)
                    StartCoroutine(Reload(type));
            }
            else
            {
                if (attackTimer < gunInfo[type].attackDelay)
                {
                    attackTimer += Time.deltaTime;
                }
                else
                {
                    if (is_fire)
                    {
                        Fire(type);
                    }
                }
            }
        }
    }

    void ChangeWeapon(string beforeType, string afterType)
    {
        if (!is_reload && !is_fire)
        {
            gunInfo[beforeType].model.enabled = false;
            gunInfo[afterType].model.enabled = true;
            PlayerInfo.Instance.gtype = (GunType)System.Enum.Parse(typeof(GunType), afterType);
        }
    }

    void Fire(string type)
    {
        if (type == null)
        {
            Debug.Log("Can't find GunInfo");
            return;
        }

        if (type != "Shotgun")
        {
            GameObject bullet = ObjectPool.Instance.PopFromPool("Bullet");
            bullet.transform.rotation = gunInfo[type].Point.rotation;
            bullet.transform.position = gunInfo[type].Point.position;
            bullet.SetActive(true);
        }

        else
        {
            for (int i = 0; i < 7; i++)
            {
                GameObject bullet = ObjectPool.Instance.PopFromPool("Bullet");
                bullet.transform.rotation = gunInfo[type].Point.rotation;
                bullet.transform.position = gunInfo[type].Point.position;
                float range = Random.Range(-10f, 10f);
                bullet.transform.rotation *= Quaternion.Euler(0f, range, 0f);
                bullet.SetActive(true);
            }
        }

        gunInfo[type].remainingMagazines -= 1;
        attackTimer = 0.0f;
        effect.gameObject.transform.position = gunInfo[type].Point.position;
        effect.gameObject.transform.rotation = gunInfo[type].Point.rotation;
        PlayShootEffect();

        if (type == GunType.AK.ToString())
        {
            PlaySoundClip("AkFire01");
        }

        else if (type == GunType.Sniper.ToString())
        {
            PlaySoundClip("SniperFire01");
        }

        else if (type == GunType.Shotgun.ToString())
        {
            PlaySoundClip("ShotgunFire01");
        }
    }

    void PlayShootEffect()
    {
        if (effect.isPlaying == true)
        {
            effect.Stop();
            effect.Clear();
        }

        else
        {
            effect.Play();
        }
    }

    public void Dead()
    {
        SceneManager.Instance.SetStageState(SceneManager.STAGE_STATE.GAMEOVER);
        SceneManager.Instance.LoadScene("GameOver");
    }

    public void Hit()
    {
        StartCoroutine(HitAnim());
        PlayerInfo.Instance.playerBaseInfo.hp -= 1;
    }

    IEnumerator HitAnim()
    {
        PlaySoundClip("PlayerHit");
        hitImg.color = new Color(1f, 0f, 0f, 0.7f);
        for (int i = 7; i >= 0; i--)
        {
            hitImg.color = new Color(1f, 0f, 0f, i * 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
