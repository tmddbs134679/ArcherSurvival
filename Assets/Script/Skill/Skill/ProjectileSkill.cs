using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class ProjectileSkill : BaseSkill
{
    PlayerSFXControl sFXControl;

    protected override void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Init();
    }

    protected override void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init();
    }

    protected override void Init()
{
    if (gameObject.GetComponentInParent<PlayerController>() != null)
    {
        SkillOwner = PlayerController.Instance.gameObject;
    }
    else
    {
        SkillOwner = gameObject;
        
    }
            SetSkillData();//????????????ш낄援ο쭛????濚밸Ŧ???
    sFXControl = GetComponentInParent<PlayerSFXControl>();  
}


    public override void SetSkillData()
    {
        Data = new ChangedSkillData();

        skillLevelSystem = GameManager.Instance.skillLevelSystem;
        Data.speed = skillLevelSystem.changedSkillData[serialname].speed;
        Data.damage = skillLevelSystem.changedSkillData[serialname].damage;
        Data.duration = skillLevelSystem.changedSkillData[serialname].duration;
        Data.color = skillLevelSystem.changedSkillData[serialname].color;
        Data.impactEffect = skillLevelSystem.changedSkillData[serialname].impactEffect;
        Data.rotateSpeed = skillLevelSystem.changedSkillData[serialname].rotateSpeed;
        Data.count = skillLevelSystem.changedSkillData[serialname].count;
        Data.angle = skillLevelSystem.changedSkillData[serialname].angle;
        Data.hormingStartDelay = skillLevelSystem.changedSkillData[serialname].hormingStartDelay;
        Data.hormingTurnDelay = skillLevelSystem.changedSkillData[serialname].hormingTurnDelay;
    }


    protected override void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            StartCoroutine(FireWithDelay());
            fireTimer = 0;
        }

    }


    //public void Fire(int count,GameObject SkillOwner,GameObject Target)
    //{

    //    SkillOwner.GetComponentInChildren<WeaponController>().AttackAni();
    //    //serialname
    //    GameObject projectile = ProjectileObjectPool.Instance.Get(projectilePrefab.name); 

    //    projectile.transform.position = SkillOwner.transform.position;
    //    projectile.transform.rotation = Quaternion.identity;

    //    Vector2 dir = Target.transform.position - SkillOwner.transform.position;
    //    Vector2 angleDir = Quaternion.Euler(0, 0, -(Data.angle * Data.count / 2f) + Data.angle * count) * dir; //

    //    projectile.GetComponent<Projectile>().Init(SkillOwner,Target, angleDir, Data);
    //}

    public void FireStart(int count, GameObject obj, GameObject Target)
    {
        StartCoroutine(Fire(count,obj, Target));
    }
    public IEnumerator Fire(int count, GameObject SkillOwner, GameObject Target)
    {
        yield return new WaitForSeconds(1f); // 1초 대기

        if (GetComponentInChildren<WeaponController>() != null)
            SkillOwner.GetComponentInChildren<WeaponController>().AttackAni();

        GameObject projectile = ProjectileObjectPool.Instance.Get(projectilePrefab.name);

        projectile.transform.position = SkillOwner.transform.position;
        projectile.transform.rotation = Quaternion.identity;

        Vector2 dir = Target.transform.position - SkillOwner.transform.position;
        Vector2 angleDir = Quaternion.Euler(0, 0, -(Data.angle * Data.count / 2f) + Data.angle * count) * dir;

        projectile.GetComponent<Projectile>().Init(SkillOwner, Target, angleDir, Data);

        if (sFXControl != null) 
        { 
            sFXControl.OnAttack(serialname);
        }
    }
    protected IEnumerator FireWithDelay()
    {
        for (int i = 0; i < Data.count; i++)
        {
            GameObject TargetTemp=null;
            
              if (SkillOwner.layer == LayerMask.NameToLayer("Player")) //SkillOwner???ル봿?? ?????????욱룑???嚥싲갭큔?댁빢彛??????????
              {
                TargetTemp = SkillOwner.GetComponent<PlayerTargeting>().GetClosestEnemy()?.gameObject;
              }
            else
            {
                TargetTemp = GetComponent<EnemyStateMachine>().Player;//?????밸븶?????轅붽틓??????우ク??
            }
            if (TargetTemp == null) yield break;
            StartCoroutine(Fire(i,SkillOwner,TargetTemp));
            yield return new WaitForSeconds(individualFireRate);

        }
    }


}
