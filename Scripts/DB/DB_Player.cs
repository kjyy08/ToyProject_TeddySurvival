using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Player : MonoBehaviour
{
    [System.Serializable]
    public class WeaponStat
    {
        public string gunType;
        public Transform Point;
        public Renderer model;
        public int remainingMagazines;
        public int maxMagazines;
        public float attackDelay;
        public float reloadDelay;
    }

    [System.Serializable]
    public class PlayerStat
    {
        public bool isDead = false;
        public bool isDamaged = false;
        public float moveSpeed;
        public float runSpeed;
        public int hp;
    }

    public Player_Control.GunType gtype;
    public PlayerStat playerStat;
}
