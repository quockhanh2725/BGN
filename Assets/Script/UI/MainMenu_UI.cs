using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_UI : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private VolumeControler_UI[] volumeControler;

    private void Start()
    {
        bool showButton = PlayerPrefs.GetInt("Level" + 2 + "Unlocked") == 1;   
        continueButton.SetActive(showButton);


        for (int i = 0; i < volumeControler.Length; i++)
        {
            volumeControler[i].GetComponent<VolumeControler_UI>().SetUpVolumeSlider();
        }
    }

    public void SwitchMenuTo(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        AudioManager.instance.PlaySFX(5);
        uiMenu.SetActive(true);
    }

    public void SetGameDiffculty(int i) => GameManager.instance.difficulty = i;
}
