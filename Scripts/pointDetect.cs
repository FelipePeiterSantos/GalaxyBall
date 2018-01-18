using UnityEngine;
using System.Collections;

public class pointDetect : MonoBehaviour {

    public bool isRightSide;
    public scoreHUD scoreScript;
    public BoxCollider child;
    public UnityEngine.UI.Text plus1Txt;
    public BoxCollider detectColl;

    bool p1Turn;
	void OnEnable () {
	    if(detectColl == null) {
            detectColl = GetComponent<BoxCollider>();
        }
        if(child == null) {
            detectColl.enabled = false;
            plus1Txt.color = Color.clear;
        }
        p1Turn = true;
	}
    
    void OnTriggerEnter(Collider coll) {
        if(coll.name == "ball") {
            if(child) {
                child.enabled = true;
            }
            else {
                scoreScript.AddPointToPlayer();
                SFXManager.PlaySFX(0,isRightSide);
                p1Turn = !p1Turn;
                StartCoroutine("plus1Feed");
            }
        }
    }

    IEnumerator plus1Feed() {
        plus1Txt.color = Color.green;
        float reduceAlpha = 0f;
        yield return new WaitForSeconds(1f);
        while(reduceAlpha < 1f) {
            reduceAlpha += 0.05f;
            plus1Txt.color = Color.green - new Color(0,0,0,reduceAlpha);
            yield return false;
        }
        yield return true;
        plus1Txt.color = Color.clear;
        detectColl.enabled = false;
    }
}
