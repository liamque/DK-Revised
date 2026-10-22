using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private GameObject gameManage;
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI creditText;

    void Start()
    {
        gameManage = GameObject.Find("GameManager");
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        counterText.text = "Lives: " + gameManage.GetComponent<GameManager>().lives;

        if (gameManage.GetComponent<GameManager>().LugiMode == false)
        {
            creditText.text = "by Wheel Good Games";
        } else
        {
            creditText.text = "Welcome to <color=#00FF00>Lugi Mode</color>!";
        }
    }
}
