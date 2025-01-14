using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum PlayerorEnemy
//{
//    player,enemy
//}

public class AtackDamage : MonoBehaviour
{
    //攻撃のコライダーに付ける

    [Header("この攻撃が敵の攻撃か味方の攻撃か設定する")]
    [SerializeField] private PlayerorEnemy playerorEnemy;
    public PlayerorEnemy GetPlayerorEnemy() => playerorEnemy;

    [SerializeField] int damage = 10;
    public int GetDamage() => damage;
    [SerializeField] float knockback = 3;
    public float GetKnockback() => knockback;

    [Header("この攻撃をくらったものはダウンするかどうか")]
    [SerializeField] bool down = false;
    public bool Down() => down;

    [Header("この攻撃が当たったらこれを持っているスクリプトを破壊するか設定する")]
    [SerializeField] bool collisionDestroy = false;

    [Header("この攻撃が部屋のオブジェクトに当たっても無視するかどうか設定する")]
    [SerializeField] bool roomCollisionIgnore = false;

    ParticleSystem[] destroyEffects;

    bool hitable = false;

    [HideInInspector]public bool hit = false;

    string tagName;

    public string GetTagname()=> tagName;
    
    void Start()
    {
        destroyEffects = GetComponentsInChildren<ParticleSystem>();

        if(playerorEnemy == PlayerorEnemy.player)
        {
            tagName = "Enemy";
        }
        if(playerorEnemy == PlayerorEnemy.enemy)
        {
            tagName = "Player";
        }

        StartCoroutine(Hitable());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitDestroy(collision);
    }
    
    /// <summary>
    /// ヒットしたときに破壊する
    /// </summary>
    /// <param name="collision"></param>
    void HitDestroy(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagName) && collisionDestroy && !hit)
        {
            for(int i = 0; i < destroyEffects.Length; i++)
            {

                if(destroyEffects != null)
                {
                    foreach(ParticleSystem effect in destroyEffects)
                    {
                        var e = effect.emission;
                        e.enabled = false;
                    }
                }

            }

            hit = true;
            Destroy(this);
        }

    }

    /// <summary>
    /// ステージとかに衝突した時に破壊する
    /// </summary>
    void StageCollision()
    {
        ////貫通させたいならcollisionDestroyをfalseにする
        //if (other.gameObject.CompareTag("Room") && collisionDestroy && hitable && !hit　&& !roomCollisionIgnore)
        //{
        //    //Debug.Log(other.gameObject);
        //    if (destroyEffects != null)
        //    {
        //        foreach (ParticleSystem effect in destroyEffects)
        //        {
        //            var e = effect.emission;
        //            e.enabled = false;
        //        }
        //    }

        //    hit = true;
        //    Destroy(this);
        //}
    }



    IEnumerator Hitable()
    {
        yield return new WaitForSeconds(0.1f);
        hitable = true;
    }


}
