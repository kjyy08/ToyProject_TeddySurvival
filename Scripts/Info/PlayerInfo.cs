using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  - 개요 -
//  플레이어 관련 공유 변수 스크립트

public class PlayerInfo : Singleton<PlayerInfo>
{
    [System.Serializable]
    public class GunInfo
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
    public class PlayerBaseInfo
    {
        public bool isDead = false;
        public bool isDamaged = false;
        public float moveSpeed;
        public float runSpeed;
        public int hp;
    }

    public Player_Control.GunType gtype;
    public PlayerBaseInfo playerBaseInfo;
}
