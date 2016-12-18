using UnityEngine;
using System.Collections;
public class opacityScript : StateMachineBehaviour
{
    public float enter_1, enter_2, enter_3, enter_4;
    private GameObject mFolkloreBlack,
                        mHorrorBlack,
                        mHistoryBlack,
                        mNatureBlack,
                        mPaperParent;

    private GameObject mObj, mThemebar;
    public float yPos = 0.48f, xPos = 1.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("HEJ");
        mFolkloreBlack = GameObject.Find("black_1");
        mHorrorBlack = GameObject.Find("black_2");
        mHistoryBlack = GameObject.Find("black_3");
        mNatureBlack = GameObject.Find("black_4");
        mPaperParent = GameObject.Find("popupParent");
        mThemebar = GameObject.Find("themeBar");
        mObj = GameObject.FindGameObjectWithTag("PopupParent");
       
        mFolkloreBlack.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enter_1);
        mHorrorBlack.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enter_2);
        mHistoryBlack.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enter_3);
        mNatureBlack.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enter_4);

        //if(animator.GetBool("openMe"))  mObj.GetComponent<Animator>().SetBool("up", true);
        //if(!animator.GetBool("openMe")) mObj.GetComponent<Animator>().SetBool("up", false);

        if (enter_1 == 0)
        {
            Vector3 tempVec = new Vector3(mFolkloreBlack.transform.position.x + xPos, mFolkloreBlack.transform.position.y + yPos, mFolkloreBlack.transform.position.z+1);
            mPaperParent.transform.localPosition = tempVec;
        }
        if (enter_2 == 0)
        {
            Vector3 tempVec = new Vector3(mHorrorBlack.transform.position.x + xPos, mHorrorBlack.transform.position.y + yPos, mHorrorBlack.transform.position.z+1);
            mPaperParent.transform.localPosition = tempVec;
        }
        if (enter_3 == 0)
        {
            Vector3 tempVec = new Vector3(mHistoryBlack.transform.position.x + xPos, mHistoryBlack.transform.position.y + yPos, mHistoryBlack.transform.position.z+1);
            mPaperParent.transform.localPosition = tempVec;
        }
        if (enter_4 == 0)
        {
            Vector3 tempVec = new Vector3(mNatureBlack.transform.position.x + xPos, mNatureBlack.transform.position.y + yPos, mNatureBlack.transform.position.z+1);
            mPaperParent.transform.localPosition = tempVec;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


        //if (mThemebar.GetComponent<Animator>().GetBool("openMe")) mObj.GetComponentInChildren<Animator>().SetBool("Up", true);
        //if (!mThemebar.GetComponent<Animator>().GetBool("openMe")) mObj.GetComponentInChildren<Animator>().SetBool("Up", false);
        //Vector3 pos = new Vector3(100, 100, 100);
        //mFolkloreBlack.transform.position = pos;
        //Debug.Log(mFolkloreBlack.transform.position);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mThemebar.GetComponent<Animator>().SetBool("openMe", false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
