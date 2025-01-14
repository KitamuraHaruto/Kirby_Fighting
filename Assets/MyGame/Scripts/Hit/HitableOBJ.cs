using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerorEnemy
{
    player,enemy
}
public class HitableOBJ : MonoBehaviour
{
    [SerializeField, Header("�����Ɏ��g���v���C���[���G������͂���")]
    PlayerorEnemy playerorEnemy = PlayerorEnemy.player;
    [SerializeField] int maxHP = 100;
    [SerializeField] float invincibleTime = 1f;

    [SerializeField] float downTime = 1.5f;

    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject deathEffect;

    [SerializeField] bool debugLog = false;

    bool damageable = true;
    int currentHP = 0;
    string tagName;
    PlayerorEnemy getAtackcol;

    public int GetHP() => currentHP;
    public int GetMaxHP() => maxHP;

    bool death = false;
    public bool GetDeath() => death;

    /// <summary> �������U���Ƀq�b�g���ăm�b�N�o�b�N���Ă�����true��Ԃ� </summary>
    bool ishit = false;
    public bool GetHit() => ishit;

    bool downFrag = false;
    public bool GetDownFrag() => downFrag;

    Rigidbody2D rb;



    void Start()
    {
        damageable = true;
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        if (playerorEnemy == PlayerorEnemy.player) { tagName = "EnemyAtack"; getAtackcol = PlayerorEnemy.enemy; }
        if (playerorEnemy == PlayerorEnemy.enemy) { tagName = "Atack"; getAtackcol = PlayerorEnemy.player; }
    }


    void Update()
    {
        //rb.AddForce(new Vector2(1, 1), ForceMode2D.Impulse);
        //Debug.Log(rb.velocity);

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (debugLog)
        {
            Debug.Log(currentHP);
            Debug.Log(tagName);
        }

        if (currentHP <= 0)
        {
            Debug.Log("���ɂ܂���");
            death = true;
            rb.velocity = Vector3.zero;

        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        AtackDamage AtackCol = collision.GetComponent<AtackDamage>();

        //�G�̍U���̃R���C�_�[�ɐڐG�����Ƃ�
        if (collision.gameObject.CompareTag(tagName) && AtackCol != null)
        {
            //�����������̂��G�̂��̂������ꍇ
            if (AtackCol.GetPlayerorEnemy() == getAtackcol && damageable)
            {
                downFrag = AtackCol.Down();
                //Debug.Log("AtackCol.Down = " + AtackCol.Down());
                //�m�b�N�o�b�N����
                Vector3 dir = collision.transform.position - transform.position;
                dir.z = 0;
                //var aaa = KitamuraMethod.VectorReplaced2D(dir, AtackCol.GetKnockback(),true);
                var aaa = KitamuraMethod.VectorReplaced2D(transform.position, collision.transform.position
                    , AtackCol.GetKnockback(),true);
                //Debug.Log(aaa);
                Debug.Log("Hit " + AtackCol.GetKnockback() + " " + aaa);
                rb.AddForce(aaa, ForceMode2D.Impulse);
                StartCoroutine(KnockBackCoroutine(aaa));

                //�_���[�W����
                currentHP -= AtackCol.GetDamage();
                //Debug.Log(currentHP);
                damageable = false;
                
                HitEffect(collision.transform.position);
                
            }
        }
    }

    IEnumerator KnockBackCoroutine(Vector3 vector)
    {
        ishit = true;
        //vector.y = 0;
        //rb.velocity += vector * 5;
        //rb.AddForce(vector * 5);

        if (downFrag)
        {
            yield return new WaitForSeconds(downTime);
        }
        else
        {
            yield return new WaitForSeconds(downTime / 3);
        }
        rb.velocity = Vector3.zero;
        ishit = false;
        StartCoroutine(DamageFragCoroutine());
    }



    IEnumerator DamageFragCoroutine()
    {
        yield return new WaitForSeconds(invincibleTime);
        damageable = true;
        rb.velocity = Vector3.zero;
    }

    public void Healing(int hp)
    {
        currentHP += hp;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        //Debug.Log(currentHP);
    }

    public void ForcedDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0)
        {
            currentHP = 0;
        }
    }

    /// <summary> �q�b�g�G�t�F�N�g�𐶐����� null�`�F�b�N������Ă� </summary>
    void HitEffect(Vector3 pos)
    {
        if (hitEffect != null && currentHP > 0)
        {
            Instantiate(hitEffect, pos, transform.rotation);
        }
    }

}
