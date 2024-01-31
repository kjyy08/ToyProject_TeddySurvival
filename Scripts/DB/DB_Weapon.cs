using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Weapon : MonoBehaviour
{
    [System.Serializable]
    public class WeaponStat
    {
        //public float recoilRange;
        public string weaponType;
        public int damage;

        public WeaponStat(string name, int damage)
        {
            this.weaponType = name;
            this.damage = damage;
        }
    }

    public Dictionary<string, WeaponStat> weaponStatDictionary = new Dictionary<string, WeaponStat>();
}
