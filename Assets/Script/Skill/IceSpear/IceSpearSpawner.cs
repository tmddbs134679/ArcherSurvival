using System.Collections;
using UnityEngine;



public class IceSpearSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public Transform target;
    public float spawnHeight = 20f;

    void OnEnable()
    {
        ExplosionSkill.OnIceSpearFired += DropMeteorDelayed;
    }

    void OnDisable()
    {
        ExplosionSkill.OnIceSpearFired -= DropMeteorDelayed;
    }
    public void DropMeteorDelayed(GameObject target, float delay)
    {
        StartCoroutine(DropMeteorCoroutine(target, delay));
    }

    private IEnumerator DropMeteorCoroutine(GameObject target, float delay)
    {
        GameObject Zone = ProjectileObjectPool.Instance.Get("BlizzardWarningZone");
        Zone.transform.position = target.transform.position;
        ProjectileObjectPool.Instance.ReleaseDelayed("BlizzardWarningZone", Zone,1.8f);

        yield return new WaitForSeconds(delay * 0.7f);

        int posX = Random.Range(-10, 10);
        GameObject IceSpear = ProjectileObjectPool.Instance.Get("Blizzard");
        IceSpear.transform.position = target.transform.position + new Vector3(posX, spawnHeight, 0);
        IceSpear IceSpearScript = IceSpear.GetComponent<IceSpear>();
        IceSpearScript.targetPosition = target.transform.position;
    }
}
