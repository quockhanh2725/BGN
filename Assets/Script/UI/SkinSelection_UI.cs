using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkinSelection_UI : MonoBehaviour
{
    [SerializeField] private int[] priceForSkin;

    [SerializeField] private bool[] skinPurchased;
    private int skin_Id;


    [Header("Compononents")]
    [SerializeField] private TextMeshProUGUI bankText;
    [SerializeField] private GameObject buyButtom;
    [SerializeField] private GameObject selectButtom;
    [SerializeField] private Animator anim;


    

    private void SetupSkinInfo()
    {
        skinPurchased[0] = true;

        for (int i = 1; i < skinPurchased.Length; i++)
        {
            bool skinUnlocked = PlayerPrefs.GetInt("SkinPurchased" + i ) == 1;

            if ( skinUnlocked )
                skinPurchased[i] = true;

        }

        bankText.text = "Bank: "+ PlayerPrefs.GetInt("TotalFruitsCollected").ToString();


        buyButtom.SetActive(!skinPurchased[skin_Id]);
        selectButtom.SetActive(skinPurchased[skin_Id]);

        buyButtom.GetComponentInChildren<TextMeshProUGUI>().text = "Price: " + priceForSkin[skin_Id];


        anim.SetInteger("skinId", skin_Id);
    }

    public bool EnoughMoney()
    {
        int totalFruits = PlayerPrefs.GetInt("TotalFruitsCollected" );

        if (totalFruits > priceForSkin[skin_Id])
        {
            totalFruits -= priceForSkin[skin_Id];

            PlayerPrefs.SetInt("TotalFruitsCollected", totalFruits);

            return true;
        }
        AudioManager.instance.PlaySFX(7);
        return false;
    }

    public void NextSkin()
    {
        AudioManager.instance.PlaySFX(5);
        skin_Id++;

        if (skin_Id > 3)
            skin_Id = 0;
        SetupSkinInfo();
    }

    public void PrevSkin()
    {
        AudioManager.instance.PlaySFX(5);
        skin_Id--;
        if (skin_Id < 0)
            skin_Id = 3;
        SetupSkinInfo();
    }

    public void Buy()
    {
        if (EnoughMoney())
        {
            PlayerPrefs.SetInt("SkinPurchased"+ skin_Id,1);
            SetupSkinInfo();
        }
        else
            Debug.Log("Nap tien");

        
    }

    public void Select()
    {
        
            PlayerManager.instance.chooseSkinId = skin_Id;
            
    }

    public void SwichSelectionButton (GameObject newButton)
    {
        selectButtom = newButton;
    }

    private void OnEnable()
    {
        SetupSkinInfo();
    }

    private void OnDisable()
    {
        selectButtom.SetActive(false);
    }
}
