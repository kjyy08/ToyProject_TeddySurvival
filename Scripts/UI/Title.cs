using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public string nextScene;
    public UnityEngine.UI.Image fadeImg;
    public float FadeTime = 2f; // Fade효과 재생시간

    public float start = 0.0f;
    public float end = 0.0f;
    public float time = 0f;
    public bool isPlaying = false;
    public bool isStart = false;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isStart = true;
            InStartFadeAnim();
        }

        if (isStart && !isPlaying)
        {
            SceneManager.Instance.SetStageState(SceneManager.STAGE_STATE.PLAYING);
            SceneManager.Instance.LoadScene(nextScene);
        }
    }

    public void OutStartFadeAnim()
    {
        if (isPlaying == true) //중복재생방지
        {
            return;
        }

        start = 1f;
        end = 0f;
        StartCoroutine("FadeOutPlay");    //코루틴 실행
    }

    public void InStartFadeAnim()
    {
        if (isPlaying == true) //중복재생방지
        {
            return;
        }

        start = 0f;
        end = 1f;
        StartCoroutine("FadeInPlay");
    }

    IEnumerator FadeInPlay()
    {
        isPlaying = true;
        Color fadecolor = fadeImg.color;
        time = 0f;
        fadecolor.a = Mathf.Lerp(start, end, time);

        while (fadecolor.a < 1f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            yield return null;
        }

        isPlaying = false;
    }

    IEnumerator FadeOutPlay()
    {
        isPlaying = true;
        Color fadecolor = fadeImg.color;
        time = 0f;
        fadecolor.a = Mathf.Lerp(start, end, time);

        while (fadecolor.a > 0f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            yield return null;
        }

        isPlaying = false;
    }
}
