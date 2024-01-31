using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioClips
{
    public string name;
    public AudioClip clip;
}


public class SoundManager : Singleton<SoundManager>
{
    public AudioClips[] m_audioClips;
    public bool allMute = false;

    public AudioClip GetAudioClip(string _name)
    {
        for (int i = 0; i < m_audioClips.Length; i++)
        {
            if (m_audioClips[i].name == _name)
            {
                return m_audioClips[i].clip;
            }
        }

        Debug.Log(_name + " Sound Clip을 찾지 못했습니다.");
        return null;
    }

    private void MuteCheck()
    {
        if (allMute)
            AudioListener.volume = 0f;
        else
            AudioListener.volume = 1f;
    }

    private void Awake()
    {
        StartCoroutine(VolumeCheck());
    }

    IEnumerator VolumeCheck()
    {
        while (true)
        {
            MuteCheck();

            yield return new WaitForSeconds(1.0f);
        }
    }
}
