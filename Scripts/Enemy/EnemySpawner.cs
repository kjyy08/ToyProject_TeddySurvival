using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] points = null;
    public static int currentEnemyNum;
    public int currentRound = 0;
    public int clearRound = 0;

    public TextMeshProUGUI roundTxt;
    public GameObject objClearBtn;

    void Start()
    {
        currentEnemyNum = 0;
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        while (SceneManager.Instance.GetStageState() == SceneManager.STAGE_STATE.PLAYING)
        {
            //  모든 적이 없을 경우 다음 라운드로 진행
            if (currentEnemyNum <= 0)
            {
                //  라운드 시작 전 2초간 대기
                yield return new WaitForSeconds(2f);
                //  라운드 변경시 UI 보여주려면 아래에 코드 작성
                currentRound++;
                
                if (currentRound >= clearRound)
                {
                    objClearBtn.SetActive(true);
                    roundTxt.text = "Game Clear!!!";
                    roundTxt.color = new Color(1f, 1f, 1f, 1f);
                    break;
                }

                roundTxt.text = "Round " + currentRound;
                StartCoroutine(PlayRoundAnim());

                //  몬스터 생성
                foreach (DB_Monster_Spawn.SpawnData i in DBManager.Instance.m_monsterSpawn.spawnTable)
                {
                    if (i.round == currentRound)
                    {
                        StartCoroutine(SpawnTarget(i.name, i.spawnNum, i.spawnDelay));
                        yield return new WaitForSeconds(1f);
                        continue;
                    }
                }
            }

            yield return null;
        }
    }

    IEnumerator SpawnTarget(string target, int num, float delay)
    {
        for(int i = 0; i < num; i++)
        {
            int pointIdx = Random.Range(0, points.Length);
            GameObject enemy = ObjectPool.Instance.PopFromPool("Enemy_" + target);
            
            enemy.transform.position = points[pointIdx].position;
            enemy.transform.rotation = points[pointIdx].rotation;
            enemy.SetActive(true);
            currentEnemyNum++;

            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator PlayRoundAnim()
    {
        for (float i = 0f; i <= 100f; i += 10f)
        {
            roundTxt.color = new Color(1f, 1f, 1f, i * 0.01f);
            yield return new WaitForSeconds(0.1f);
        }

        for (float i = 100f; i >= 0f; i -= 2f)
        {
            roundTxt.color = new Color(1f, 1f, 1f, i * 0.01f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
