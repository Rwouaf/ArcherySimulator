//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Enemy : MonoBehaviour, IEnemy
//{
//    [SerializeField]
//    private int hp;
//    public int HP { get { return hp; } set { hp = value; } }

//    public Enemy(int hp)
//    {
//        HP = hp;
//    }
//    public void OnCollisionEnter(Collision other)
//    {
//        if (other.gameObject.CompareTag("weapons"))
//        {
//            Debug.Log("Hit");
//            var weapon = other.gameObject.GetComponent<IWeapon>();
//            if (weapon != null)
//            {
//                weapon.Attack(this);
//            }
//        }
//    }
//}
