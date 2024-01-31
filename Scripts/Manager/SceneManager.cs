using QuantumTek.QuantumUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scene = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : Singleton<SceneManager>
{
    public enum STAGE_STATE { AWAKE, TITLE, PLAYING, CLEAR, GAMEOVER, PAUSE, RESUME, EXIT };

    [SerializeField] private STAGE_STATE m_StageState = STAGE_STATE.AWAKE;
    
    public void SetStageState(STAGE_STATE _state)
    {
        m_StageState = _state;
    }

    public STAGE_STATE GetStageState()
    {
        return m_StageState;
    }

    public void LoadScene(string next)
    {
        Scene.LoadScene(next);
    }

    public void PlayScene()
    {
        SetStageState(STAGE_STATE.PLAYING);
    }

    public void PauseScene()
    {
        if (m_StageState == STAGE_STATE.PLAYING)
        {
            m_StageState = STAGE_STATE.PAUSE;
            Time.timeScale = 0f;
        }
        else if (m_StageState == STAGE_STATE.PAUSE)
        {
            m_StageState = STAGE_STATE.PLAYING;
            Time.timeScale = 1f;
        }
    }
}