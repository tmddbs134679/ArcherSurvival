using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Attack Info")]

    [SerializeField] private SpriteRenderer weaponRenderer;

    [SerializeField] private float delay = 1f;
    public float Delay { get => delay; set => delay = value; }

    private Animator animator;

    private static readonly int isAttack = Animator.StringToHash("isAttack");

    PlayerTargeting playerTargeting;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerTargeting = GetComponentInParent<PlayerTargeting>();
        animator.speed = 1.8f;
    }

    private void Update()
    {
        RotateWeaponToTarget();
    }

    public void FlipWeapon(bool isLeft)
    {
        weaponRenderer.flipY = isLeft;
    }


    public void AttackAni()
    {
        animator.SetTrigger(isAttack);
    }

    // ?寃?媛먯??섎㈃ ?寃잛そ?쇰줈 sprite ?뚯쟾 諛?flip
    void RotateWeaponToTarget()
    {
        if (playerTargeting == null)
        {
            RotateWeapon(-90f);
            return;
        }
        Transform target = playerTargeting.GetClosestEnemy();
        Vector2 targetPosition = playerTargeting.EnemyDirection();
        if (target != null)
        {
            float rotation = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            RotateWeapon(rotation);

            bool isLeft = Mathf.Abs(rotation) > 90f;
            FlipWeapon(isLeft);
        }
    }

    public void RotateWeapon(float rotation)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }

}
