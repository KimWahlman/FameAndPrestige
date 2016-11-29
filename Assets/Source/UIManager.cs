using UnityEngine;
using System.Collections;
using UnityEngine.UI;


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
        /*Debug.Log(gameManager.myPlayer.Ink + " ink cost shame : " + shameAction.InkCost);
        if(gameManager.myPlayer.Ink >= shameAction.InkCost)
            foreach(var bt in ShameButtons)
                bt.interactable = true;
        else
            foreach (var bt in ShameButtons)
                bt.interactable = false;

        Debug.Log(gameManager.myPlayer.Ink + " ink cost duel : " + duelAction.InkCost);
        if (gameManager.myPlayer.Ink >= duelAction.InkCost)
            foreach (var bt in DuelButtons)
                bt.interactable = true;
        else
            foreach (var bt in DuelButtons)
                bt.interactable = false;*/

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
        if(gameManager.myPlayer.Ink >= shameAction.cost)
        {
            foreach (var bt in ShameButtons)
                bt.interactable = true;
        }
        else
        {
            foreach (var bt in ShameButtons)
                bt.interactable = false;
        }

        if (gameManager.myPlayer.Ink >= duelAction.cost)
        {
            foreach (var bt in DuelButtons)
                bt.interactable = true;
        } else
        {
            foreach (var bt in DuelButtons)
                bt.interactable = false;
        }

    }




}
