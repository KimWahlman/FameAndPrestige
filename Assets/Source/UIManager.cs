﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

    public GameManager gameManager;
    public GameObject SelectionPanel;

    public Button[] DuelButtons;
    public Button[] ShameButtons;

    public Button[] ThemesButtons;
    public GameObject ThemeBtPanel;

    public ShameAction shameAction;
    public DuelAction duelAction;

    private GameObject selectedAction;
    Actions actionScript;

    public Button EndTurnBt;
    public Button PlayCardsBt;
    public Button ActionBt;

	public Image bubble;
    public GameObject YourTurnToPlayText;
    public GameObject TimeLeft;
    public Text TimeLeftText;
    float timerDelay = 10.0f;

    public void ShowSelectButtons()
    {
        SelectionPanel.SetActive(!SelectionPanel.activeInHierarchy);
    }

    public void CloseThemeUI()
    {
        ThemeBtPanel.SetActive(false);
    }
    
    public void ActionsBt()
    {
        ShowSelectButtons();
        CloseThemeUI();
    }

    public void AssignActionToTheme(Actions.actionEnum action)
    {
        selectedAction = new GameObject();

        switch (action)
        {
            case Actions.actionEnum.SHAME:
                selectedAction.AddComponent<ShameAction>();
                actionScript = selectedAction.GetComponent<ShameAction>();
                break;
            case Actions.actionEnum.DUEL:
                selectedAction.AddComponent<DuelAction>();
                actionScript = selectedAction.GetComponent<DuelAction>();
                break;
        }

        Destroy(selectedAction);

        ThemeBtPanel.SetActive(true);

        foreach (var bt in ThemesButtons)
        {
            string tmpName = bt.name;
            bt.onClick.RemoveAllListeners();
            bt.onClick.AddListener(() => actionScript.useAction(tmpName));
            bt.onClick.AddListener(() => CloseThemeUI());
            bt.onClick.AddListener(() => ShowSelectButtons());
        }
    }

    public void CheckAvailableActions()
    {

        Debug.Log("my ink = " + gameManager.myPlayer.Ink + " shame cost : " + shameAction.getCost() + " duel cost : " + duelAction.getCost());

        if(gameManager.myPlayer.Ink >= shameAction.getCost())
        {
            foreach (var bt in ShameButtons)
                bt.interactable = true;
        }
        else
        {
            foreach (var bt in ShameButtons)
                bt.interactable = false;
        }

        if (gameManager.myPlayer.Ink >= duelAction.getCost())
        {
            foreach (var bt in DuelButtons)
                bt.interactable = true;
        } else
        {
            foreach (var bt in DuelButtons)
                bt.interactable = false;
        }

    }

<<<<<<< HEAD
	public void ShowBubble(string message){
	
		bubble.gameObject.SetActive (true);
		bubble.GetComponentsInChildren<Text>()[0].text = message;
	}

	public void HideBubble(){

		StartCoroutine (hideBubble ());
	}
=======
    public void StartTime()
    {
        timerDelay = 10.0f;
        TimeLeft.SetActive(true);
    }

    public void StopTimer()
    {
        TimeLeft.SetActive(false);
    }
    
    void Update()
    {
        if(TimeLeft.activeInHierarchy)
        {
            timerDelay -= Time.deltaTime;

            TimeLeftText.text = "Time Left : " + Mathf.Round(timerDelay);
        }
    }
>>>>>>> origin/Iann

	IEnumerator hideBubble(){

		yield return new WaitForSeconds (2f);
		bubble.GetComponentsInChildren<Text> () [0].text = "";
		bubble.gameObject.SetActive (false);
	}

}
