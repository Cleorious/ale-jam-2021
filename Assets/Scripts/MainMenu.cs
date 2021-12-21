using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Transform root;
    public Button playButton;
    public TMP_InputField playerCountInputField;
    GameManager gameManager;
    int playerCount = 3;
    
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        playButton.onClick.AddListener(OnPlayButtonClicked);
        playerCountInputField.onEndEdit.AddListener((string s) =>
        {
            int.TryParse(s, out playerCount);
            if(playerCount > Parameter.CHEF_FACES_COUNT)
            {
                playerCount = Parameter.CHEF_FACES_COUNT;
            }
            else if(playerCount < 2)
            {
                playerCount = 2;
            }

            playerCountInputField.text = playerCount.ToString();
        });
    }

    public void OnPlayButtonClicked()
    {
        gameManager.PlayGame(playerCount);
        root.gameObject.SetActive(false);
    }
}
