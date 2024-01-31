using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 50f;
    public string poolItemName = "Bullet";
    public float lifeTime = 3f;
    public float _elapsedTime = 0f;
    private Rigidbody _rigidbody;
    private TrailRenderer _trailRenderer;
    public float recoilRange = 0f;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    void OnEnable()
    {
        //if (SceneManager.Instance.GetStageState() != SceneManager.STAGE_STATE.AWAKE)
        //{
            _trailRenderer.enabled = true;
            //string type = PlayerInfo.Instance.gtype.ToString();
            //float range = Random.Range(-recoilRange, recoilRange);
            //transform.rotation *= Quaternion.Euler(0f, range, 0f);
            //_rigidbody.AddForce(transform.forward * moveSpeed);
        //}
    }

    void OnDisable()
    {
        _trailRenderer.enabled = false;
    }

    float GetTimer()
    {
        return (_elapsedTime += Time.deltaTime);
    }

    void SetTimer(float val = 0f)
    {
        _elapsedTime = val;
    }

    void PushPoolBullet()
    {
        SetTimer();
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
        //  _rigidbody.velocity = Vector3.zero;
        //  _rigidbody.angularVelocity = Vector3.zero;
        ObjectPool.Instance.PushToPool(poolItemName, gameObject);
    }

    void Update()
    {
        if (GetTimer() > lifeTime)
        {
            PushPoolBullet();
            return;
        }

        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            return;
        }

        if (!other.CompareTag("Enemy") || PlayerInfo.Instance.gtype != Player_Control.GunType.Sniper)
        {
            SetTimer(lifeTime - 0.01f);
        }
    }
}
