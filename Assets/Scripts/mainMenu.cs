using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[Serializable]
public class userScore
{
    public float score;
    public string _name;

    public userScore(string _name, float score)
    {
        this._name = _name;
        this.score = score;
    }
}

public class mainMenu : MonoBehaviour
{
    

    [SerializeField]
    private Slider mouseSensivitySlider;
    [SerializeField] private TextMeshProUGUI mouseSensivityText;
    [SerializeField]
    private TMP_InputField nameInput;
    [SerializeField]
    private Animator nameInputShakeAnim;
    [SerializeField]
    private GameObject leaderboardNamePrefab;
    [SerializeField]
    private RectTransform userParent;
    public List<userScore> scores = new List<userScore>();
    private void Start()
    {
        mouseSensivitySlider.value = PlayerPrefs.GetFloat("sensivity",1000);
        mouseSensivityText.text = mouseSensivitySlider.value.ToString();

        PlayerPrefs.DeleteKey("currentName");

        loadScoreData();
        showLeaderboard();
    }
    public void loadScoreData()
    {
        scores.Clear();
        int scoreCount = PlayerPrefs.GetInt("scoreCount", 0);
        for (int i = 0; i < scoreCount; i++)
        {
            string _name = PlayerPrefs.GetString("scoreName" + i);
            int score = PlayerPrefs.GetInt("score" + i);
            scores.Add(new userScore(_name, score));
        }
    }
    public void startButtonClick()
    {
        string name = nameInput.text;
        name.Replace(" ", "");
        if (name != null && name != "")
        {
            PlayerPrefs.SetString("currentName",name);
            SceneManager.LoadScene("mainScene");
        }else
        {
            nameInputShakeAnim.SetTrigger("start");
        }
    }
    public void showLeaderboard()
    {
        List<userScore> newScores = new List<userScore>();
        newScores.AddRange(scores);
        List<userScore> topScores = new List<userScore>();

        for (int i = 0; i < 5; i++)
        {
            int enBuyukNo = -99;
            if (newScores.Count > 0)
            {
                for (int x = 0; x < newScores.Count; x++)
                {
                    if (enBuyukNo == -99)
                    {
                        enBuyukNo = x;
                    }else if (newScores[x].score > newScores[enBuyukNo].score)
                    {
                        enBuyukNo = x;
                    }
                }
            }else
            {
                break;
            }
            Debug.Log(newScores[enBuyukNo]._name);
            topScores.Add(newScores[enBuyukNo]);
            newScores.RemoveAt(enBuyukNo);
        }

        for (int i = 0; i < topScores.Count; i++)
        {
            GameObject userObj = Instantiate(leaderboardNamePrefab);
            userObj.transform.SetParent(userParent);
            userObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-3,230 - (100 * i));
            userObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString() + " -";
            userObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = topScores[i]._name + " / " + topScores[i].score;
        }
    }
    public void sensivityChange()
    {
        PlayerPrefs.SetFloat("sensivity",mouseSensivitySlider.value);
        mouseSensivityText.text = mouseSensivitySlider.value.ToString();
    }
}
