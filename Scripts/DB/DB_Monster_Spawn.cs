using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Monster_Spawn : MonoBehaviour
{
    public class SpawnData
    {
        public int round;
        public string name;
        public int spawnNum;
        public float spawnDelay;

        public SpawnData(int round, string name, int spawnNum, float spawnDelay)
        {
            this.round = round;
            this.name = name;
            this.spawnNum = spawnNum;
            this.spawnDelay = spawnDelay;
        }
    }

    public List<SpawnData> spawnTable = new List<SpawnData>();
}
