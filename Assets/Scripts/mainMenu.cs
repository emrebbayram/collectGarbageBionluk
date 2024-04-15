using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour
{
    [SerializeField]
    private Slider mouseSensivitySlider;
    [SerializeField] private TextMeshProUGUI mouseSensivityText;
    private void Start()
    {
        mouseSensivitySlider.value = PlayerPrefs.GetFloat("sensivity",1000);
        mouseSensivityText.text = mouseSensivitySlider.value.ToString();
    }
    public void startButtonClick()
    {
        SceneManager.LoadScene("mainScene");
    }
    public void sensivityChange()
    {
        PlayerPrefs.SetFloat("sensivity",mouseSensivitySlider.value);
        mouseSensivityText.text = mouseSensivitySlider.value.ToString();
    }
}
