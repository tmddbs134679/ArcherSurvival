using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUI : MonoBehaviour
{
    [SerializeField]
    private PlayerResource playerResource;
    [SerializeField]
    private Text goldAmountText;
    private void Start()
    {
        playerResource = PlayerController.Instance.GetComponent<PlayerResource>();
        if(playerResource != null ) 
        {
            playerResource.OnGoldChanged += UpdateUI;
            UpdateUI();
        }
    }

    private void OnDestroy()
    {
        playerResource.OnGoldChanged -= UpdateUI;
    }

    public void UpdateUI()
    {
        int gold = playerResource.goldAmount;
        goldAmountText.text = gold.ToString();
    }
}
