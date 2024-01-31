using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Monster : MonoBehaviour
{
    public class MonsterStat
    {
        public string type;
        public float moveSpeed;
        public float runSpeed;
        public int hp;
        public int damage;

        public MonsterStat(string name, int hp, int damage, float moveSpeed, float runSpeed)
        {
            this.type = name;
            this.moveSpeed = moveSpeed;
            this.runSpeed = runSpeed;
            this.hp = hp;
            this.damage = damage;
        }
    }

    public Dictionary<string, MonsterStat> monsterStatDictionary = new Dictionary<string, MonsterStat>();

    public void IncreaseAllEnemyTypeStats(int value)
    {
        foreach (KeyValuePair<string, MonsterStat> i in monsterStatDictionary)
        {
            i.Value.hp += value;
            i.Value.damage += value;
        }
    }

    public void IncreaseOneEnemyTypeStats(string type, string stats, int value)
    {
        switch (stats)
        {
            case "hp":
                monsterStatDictionary[type].hp += value;
                break;
            case "damage":
                monsterStatDictionary[type].damage += value;
                break;
        }
    }
}
