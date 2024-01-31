using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame : Singleton<InGame>
{
    public Text txtBox_Round;
    public Image imgHit;

    private Animator anim;

    public int currentRound = 0;
    public int previousRound = 0;
    public bool isRoundChange = false;

    void TxtChangeAnimStop()
    {
        anim.speed = 1f;
        anim.SetBool("isChangeRound", false);
    }

    void ImgFadeAnimStop()
    {
        anim.speed = 1f;
        SceneManager.Instance.SetStageState(SceneManager.STAGE_STATE.GAMEOVER);
        SceneManager.Instance.LoadScene("GameOver");
    }

    void ImgFadeInit()
    {
        anim.SetBool("isGameStart", true);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentRound = 0;
        previousRound = currentRound;
        StartCoroutine(ShowCurrentRound());
    }

    IEnumerator IsHit()
    {
        float timer = 0f;

        while (timer >= 0.5f)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        PlayerInfo.Instance.playerBaseInfo.isDamaged = false;
    }

    private void Update()
    {
        if (PlayerInfo.Instance.playerBaseInfo.isDamaged)
        {
            StartCoroutine(IsHit());
        }
    }

    IEnumerator ShowCurrentRound()
    {
        while (SceneManager.Instance.GetStageState() == SceneManager.STAGE_STATE.PLAYING)
        {
            if (isRoundChange)
            {
                isRoundChange = false;
                txtBox_Round.text = "Round : " + currentRound.ToString();
                anim.speed = 0.3f;
                anim.SetBool("isChangeRound", true);
            }

            if (PlayerInfo.Instance.playerBaseInfo.isDead)
            {
                anim.speed = 0.3f;
                anim.SetBool("isGameOver", true);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
