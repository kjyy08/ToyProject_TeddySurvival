using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class DBManager : Singleton<DBManager>
{
    private string m_DatabaseFilePath;
    private DB_Monster db_Monster = new DB_Monster();
    private DB_Weapon db_Weapon = new DB_Weapon();
    private DB_Monster_Spawn db_Monster_Spawn = new DB_Monster_Spawn();

    public DB_Monster m_monster { get { return db_Monster; } }
    public DB_Weapon m_weapon { get {  return db_Weapon; } }
    public DB_Monster_Spawn m_monsterSpawn { get { return db_Monster_Spawn; } }


    void Awake()
    {
        m_DatabaseFilePath = "Data Source=" + Application.streamingAssetsPath + "/TeddySuv.db";
        string connectionString = m_DatabaseFilePath;

        DBAccess m_dbAccess = new DBAccess(connectionString);

        ReadMonsterDB(ref m_dbAccess);
        ReadWeaponDB(ref m_dbAccess);
        ReadMonsterSpawnDB(ref m_dbAccess);

        m_dbAccess.CloseSqlConnection();
    }

    void ReadMonsterDB(ref DBAccess access)
    {
        string tableName = "Monster";
        IDataReader reader = access.ReadFullTable(tableName);

        while (reader.Read())
        {
            string name = reader.GetString(1);
            int hp = reader.GetInt32(2);
            int damage = reader.GetInt32(3);
            float moveSpeed = reader.GetFloat(4);
            float runSpeed = reader.GetFloat(5);
            
            db_Monster.monsterStatDictionary[name] = new DB_Monster.MonsterStat(name, hp, damage, moveSpeed, runSpeed);
        }
    }
    void ReadMonsterSpawnDB(ref DBAccess access)
    {
        string tableName = "Monster_Spawn";
        IDataReader reader = access.ReadFullTable(tableName);

        while (reader.Read())
        {
            string name = reader.GetString(0);
            int round = reader.GetInt32(1);
            int spawnNum = reader.GetInt32(2);
            float spawnDelay = reader.GetFloat(3);

            db_Monster_Spawn.spawnTable.Add(new DB_Monster_Spawn.SpawnData(round, name, spawnNum, spawnDelay));
        }
    }

    void ReadWeaponDB(ref DBAccess access)
    {
        string tableName = "Weapon";
        IDataReader reader = access.ReadFullTable(tableName);

        while (reader.Read())
        {
            string name = reader.GetString(1);
            int damage = reader.GetInt32(2);
            
            db_Weapon.weaponStatDictionary[name] = new DB_Weapon.WeaponStat(name, damage);
        }
    }

    
}
