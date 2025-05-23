using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    [SerializeField]
    private int stage = 0;
    [SerializeField]
    private float time;
    [SerializeField]
    public bool isOpen = false;
    public GameObject[] entities;
    [SerializeField]
    private int enemyCount = 0;
    [SerializeField]
    public int roomCount = 1;
    public GameObject[] rooms;
    public GameObject[] bossRooms;
    public GameObject lodingPrefab;
    public GameObject lodingObject;
    public SkillLevelSystem skillLevelSystem;

    public bool isOption = false;
    public bool isStartLoading = false;






    private void HandleMonsterDeath(GameObject monster)
    {
        enemyCount--;
        MonsterPoolManager.Instance.ReturnObject(monster, monster.name);
        //CheckEnemy();
    }
    protected override void Awake()
    {
        base.Awake();
        if (GameManager.Instance == this)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            rooms = Resources.LoadAll<GameObject>("Prefabs/Stages/Room");
            bossRooms = Resources.LoadAll<GameObject>("Prefabs/Stages/BossRoom");

            lodingObject = GameObject.Find("Loading");
            
            //????????????Β?щ열???
            //??????袁⑸즴筌?씛彛?????
            //?????븐뼐??????????쇰뮛???ㅺ껀???
            
        }
        
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        lodingObject = GameObject.Find("Loading");
        isOpen = false;
        if (SceneManager.GetActiveScene().name == "LobbyScene")
        {
            AchievementManager.Instance.currentKillCnt = new Dictionary<string, int>();
            SoundManager.Instance.PlayTitleBGM();
        }
        
        else if (SceneManager.GetActiveScene().name == "InfiniteModeScene")
        {
            roomCount = -10;
            SoundManager.Instance.PlayDungeonBGM();
            PlayerController.Instance.transform.position = new Vector3(0, 0, 0);
        }
        
        else
        {
            Init_GameManager();   
            SoundManager.Instance.PlayDungeonBGM();
        }
        ItemPool.Instance.AllObjectOff();
        Time.timeScale = 1.0f;


        Invoke("DelayFadeOut", 0.5f);
        Invoke("CheckEnemy", 0.5f);
        

    }

    public void DelayFadeOut()
    {

        
        if (SceneManager.GetActiveScene().name != "LobbyScene")
        {
            Time.timeScale = 0f;
        }
        UIManager.Instance.FadeOutUI("Loading");
        // Debug.Log("??");


        /*
        foreach (GameObject obj in UIManager.Instance.uiObjects)
        {
            if (obj.name == "Loading")
            {
                Debug.Log("??");
                UIManager.Instance.FadeOutUI("Loading");
                return;
            }
        }

        Invoke("DelayFadeOut", 0.5f);
        */

    }
    private void Init_GameManager()
    {
        CreateRoom();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isUI = false;
            foreach(var obj in UIManager.Instance.uiObjects)
            {
                if(obj.active == true)
                {
                    isUI = true;
                }
            }

            if (isUI == false)
            {
                time += Time.deltaTime;
                OnOption();
            }
        }
    }
    public void CreateRoom()
    {
        //Debug.Log("Create Room");
        if (roomCount % 5 == 0)
        {
            Instantiate(bossRooms[UnityEngine.Random.RandomRange(0, bossRooms.Length)]);
        }
        else
        {
            Instantiate(rooms[UnityEngine.Random.RandomRange(0, rooms.Length)]);
        }
        roomCount++;
    }
    public void NextRoom()
    {
        if (isOpen)
        {
            isOpen = false;

            ProjectileObjectPool.Instance.AllObjectOff();

            if (roomCount < 6)
            {
                isStartLoading = true;
                
                UIManager.Instance.FadeInUI("Loading");
            }
            else
            {
                UIManager.Instance.ShowUI("Clear");
            }
            
        }
    }

    // LodingFadeIn => NextSceneLoad
    public void NextSceneLoad()
    {
        //LoadingManager.LoadScene("AITestScene");
        PlayerController.Instance.transform.position = new Vector3(0,0,0);
        
        SceneManager.LoadScene("AITestScene");
    }


    public void LobbySceneLoad()
    {
        roomCount = 1;
        enemyCount = 0;
        PlayerController.Instance.GetComponent<BaseStat>().Healed(10000000);
        PlayerController.Instance.GetComponentInChildren<Animator>().SetLayerWeight(2, 0);
        PlayerController.Instance.transform.position = new Vector3(0, 0, 0);

        MonsterPoolManager.Instance.AllObjectOff();

        skillLevelSystem.Init_Skill();
        /*
        foreach (var key in skillLevelSystem.skillData.Keys)
        {
            skillLevelSystem.changedSkillData[key].level = 0;
        }
        */
        foreach(Transform child in PlayerController.Instance.transform)
        {
            if (child.CompareTag("Skill"))
            {
                Destroy(child.gameObject);
            }
        }
        PlayerController.Instance.skillList = new List<GameObject>();




        SceneManager.LoadScene("LobbyScene");
    }

    public static event Action openCloseDoor;
    public void CheckEnemy()
    {
        if (enemyCount <= 0)
        {
            isOpen = true;
            openCloseDoor?.Invoke();
            //UIManager.Instance.ShowUI("Reward");
        }
    }
    public void EnemyCounting(int count)
    {
        /*
        entities = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = entities.Length;
        */
        enemyCount += count;
        CheckEnemy();
    }
    public void GameOver()
    {
        //UIManager.Instance.ShowUI("GameOverUI");
        UIManager.Instance.HideUI("Reward");
        roomCount = 6;
        Time.timeScale = 0f;
        ProjectileObjectPool.Instance.AllObjectOff();
        UIManager.Instance.ShowUI("Clear");

    }

    public void OnOption()
    {

        if (isOption == false)
        {
            isOption = true;
            Time.timeScale = 0f;
            UIManager.Instance.ShowUI("Option");
        }

        else
        {
            isOption = false;
            Time.timeScale = 1.0f;
            UIManager.Instance.HideUI("Option");
        }


    }


    public void LoadSceneLobby()
    {
        SceneManager.LoadScene("Lobby");
    }


}
