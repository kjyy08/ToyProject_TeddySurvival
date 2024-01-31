using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public string nextScene;

    public void PressHomeBtn()
    {
        SceneManager.Instance.SetStageState(SceneManager.STAGE_STATE.TITLE);
        SceneManager.Instance.LoadScene(nextScene);
    }
}
