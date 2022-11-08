using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public Toggle musicToggle;

    void Start()
    {
        musicToggle.isOn = PlayerPrefs.GetInt("Music", 1) == 1;
        Debug.Log("MC: Music is " + (musicToggle.isOn ? "on" : "off"));
        // musicSource = GetComponent<AudioSource>();
    }

    public void TrueFalseChecker()
    {
        if (musicToggle.isOn)
        {
            PlayerPrefs.SetInt("Music", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 0);
        }

        Debug.Log("MC: Music now is " + (musicToggle.isOn ? "on" : "off"));
    }
}
