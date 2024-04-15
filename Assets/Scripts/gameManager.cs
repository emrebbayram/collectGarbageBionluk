using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

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
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private Slider mouseSensivitySlider;
    [SerializeField] private TextMeshProUGUI mouseSensivityText;
    [SerializeField]
    private bool paused;

    private MouseLook _mouseLook;


    public static gameManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
        _gameEndTime = startTime + Time.time;

        Time.timeScale = 1;
    }

    private void Start()
    {
        _mouseLook = GetComponentInChildren<MouseLook>();
        mouseSensivitySlider.value = PlayerPrefs.GetFloat("sensivity", 1000);
        mouseSensivityText.text = mouseSensivitySlider.value.ToString();
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
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            pauseGameManager();
        }
    }
    public void pauseGameManager()
    {
        pauseUI.SetActive(paused);
        inGameUI.SetActive(!paused);
        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }
    public void sensivityChange()
    {
        PlayerPrefs.SetFloat("sensivity", mouseSensivitySlider.value);
        mouseSensivityText.text = mouseSensivitySlider.value.ToString();
        _mouseLook.changeSensivity(mouseSensivitySlider.value);
    }
    public void trashSpawn()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-145f,54f),10f,Random.Range(-45,92));
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
