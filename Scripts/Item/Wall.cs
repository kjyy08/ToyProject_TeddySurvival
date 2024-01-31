using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wall : MonoBehaviour
{
    public string poolItemName;
    public int hp;
    public Renderer model;
    public float FadeTime = 2f; // Fade효과 재생시간

    private float start = 1.0f;
    private float end = 0.0f;
    private float time = 0f;
    private bool isHit = false;
    private BoxCollider col;
    private NavMeshObstacle nav;
    private AudioSource sound;

    void PlaySoundClip(string name)
    {
        sound.PlayOneShot(SoundManager.Instance.GetAudioClip(name));
    }

    IEnumerator CheckWallDamaged()
    {
        while (true)
        {
            if (isHit)
            {
                isHit = false;

                if (hp <= 0)
                {
                    col.enabled = false;
                    PlaySoundClip("DestroyWall");
                    StartCoroutine(DestroyWall());
                    break;
                }

                else
                {
                    hp -= 1;
                }
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
    
    IEnumerator DestroyWall()
    {
        Color fadecolor = model.material.color;
        time = 0f;
        fadecolor.a = Mathf.Lerp(start, end, time);

        while (fadecolor.a > 0f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            model.material.color = fadecolor;
            yield return null;
        }

        nav.carving = false;
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        model = GetComponent<Renderer>();
        nav = GetComponent<NavMeshObstacle>();
        sound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        col.enabled = true;
        nav.carving = true;
        model.material.color = new Color(1f, 1f, 1f, 1f);
        StartCoroutine(CheckWallDamaged());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isHit = true;
        }

        else if (other.CompareTag("Player"))
        {
            model.material.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        model.material.color = new Color(1f, 1f, 1f, 1f);
    }
}
