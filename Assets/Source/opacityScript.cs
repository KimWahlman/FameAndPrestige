using UnityEngine;
using System.Collections;

public class opacityScript : StateMachineBehaviour
{

    public float enter_1, enter_2, enter_3, enter_4;
    //public float exit_1, exit_2, exit_3, exit_4;

    private GameObject mFolkloreBlack,
                            mHorrorBlack,
                            mHistoryBlack,
                            mNatureBlack;

    //private MonoBehaviour monobehaviour;

    public void StateMachineBehaviour(MonoBehaviour monoBehaviour)
    {
        //this.monobehaviour = monoBehaviour;
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("HEJ");
        mFolkloreBlack = GameObject.Find("black_1");
        mHorrorBlack = GameObject.Find("black_2");
        mHistoryBlack = GameObject.Find("black_3");
        mNatureBlack = GameObject.Find("black_4");
        //mFolkloreBlack.SetActive(false);
        mFolkloreBlack.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enter_1);
        mHorrorBlack.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enter_2);
        mHistoryBlack.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enter_3);
        mNatureBlack.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enter_4);
    }

   
}
