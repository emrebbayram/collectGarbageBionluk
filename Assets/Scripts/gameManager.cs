using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{

    [SerializeField]
    private float startTime;
    public float timeEarnAmount;
    [HideInInspector]
    public float _gameEndTime;
    public int point;
    public int earnPoint;
    public int finePoint;


    private static gameManager _instance;

    [SerializeField]
    private GameObject[] trashes;
    [SerializeField]
    private float trashSpawnWaitTime;
    public float nextTrashSpawnTime;
    [SerializeField]
    private LayerMask groundLayer;
    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI remainingTimeText;
    [SerializeField]
    private TextMeshProUGUI pointsText;
    [SerializeField]
    private GameObject endUI;
    [SerializeField]
    private GameObject inGameUI;


    public static gameManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
        _gameEndTime = startTime + Time.time;

        Time.timeScale = 1;
    }

    private void Update()
    {
        if (_gameEndTime != Mathf.Infinity)
        {
            remainingTimeText.text = "Kalan Süre : " + Mathf.Round(_gameEndTime - Time.time).ToString();
        }
        pointsText.text = "Puan : "+point.ToString();

        if (nextTrashSpawnTime <= Time.time)
        {
            nextTrashSpawnTime = Time.time + trashSpawnWaitTime;
            trashSpawn();
        }
        if (_gameEndTime < Time.time)
        {
            _gameEndTime = Mathf.Infinity;
            endGame();
        }
    }
    public void trashSpawn()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-44f,54f),10f,Random.Range(-45,92));
        RaycastHit hit;
        if (Physics.Raycast(spawnPos,Vector3.down, out hit, 500f, groundLayer))
        {
            print("hit");
            int randomH = Random.Range(0,trashes.Length);
            GameObject spawnedTrash = Instantiate(trashes[randomH]);
            spawnedTrash.transform.position = hit.point + new Vector3(0,trashes[randomH].GetComponent<itemObj>().item.groundDistance,0);
        }
    }
    public void endGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        endUI.gameObject.SetActive(true);
        inGameUI.SetActive(false);
    }
    public void restartGameClick()
    {
        SceneManager.LoadScene("mainScene");

    }
    public void mainMenuClick()
    {
        SceneManager.LoadScene("mainMenu");
    }

}
