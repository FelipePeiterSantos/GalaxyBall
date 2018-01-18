using UnityEngine;
using System.Collections;

public class ballthrow : MonoBehaviour {

    public bool isRightSide;
    public GameObject cameraMain;
    public scoreHUD scoreScript;
    public UnityEngine.UI.Text[] txtFeed;
    public Vector3 startPosition;
    public GameObject lineRender;
    public AnimationCurve curve;
    [System.Serializable]public struct Vec3Quat {
        public Vector3 vec3;
        public Vector3 quat;
    }
    public Vec3Quat[] cameraPositions;

    Rigidbody rig;
    MeshRenderer render;
    ConstantForce gravityForce;

    Camera cam;
    LineRenderer lr;
    Vector2 mouseStartPos;
    bool mouseDown;
    bool endRound;
    float mouseDragX;
    float mouseDragY;
    float mouseXCurrent;
    float mouseYCurrent;
    float airTime;
    bool outBounds;
    bool isRoundGoing;

    bool isMobile;
    int myFingerID;

    float velocity;
    float angle;
    public int resolution;
    float radianAngle;
    float g;

	void OnEnable () {
        lr = lineRender.GetComponent<LineRenderer>();
        if(rig == null) {
            rig = GetComponent<Rigidbody>();
        }
        rig.isKinematic = true;
        rig.useGravity = false;

        if(gravityForce == null) {
            gravityForce = GetComponent<ConstantForce>();
        }

        if(render == null) {
            render = GetComponent<MeshRenderer>();
        }
        cam = cameraMain.GetComponent<Camera>();
        outBounds = true;
        transform.localPosition = startPosition;
        mouseDown = false;
        mouseStartPos = Vector2.zero;
        mouseDragY = 0;
        lr.enabled = false;
        txtFeed[1].color = Color.clear;
        cameraMain.transform.localPosition = cameraPositions[0].vec3;
        cameraMain.transform.localEulerAngles = cameraPositions[0].quat;
        endRound = true;
#if UNITY_ANDROID
        isMobile = true;
#endif
#if UNITY_EDITOR
        isMobile = false;
#endif
    }
	
	void Update () {
        if(InputGetDown() && rig.isKinematic == true && endRound){
            mouseDown = true;
            mouseDragX = 0;
            mouseDragY = 0;
            mouseStartPos = cam.ScreenToViewportPoint(InputPosition());
        }

        if (InputGetUp() && mouseDown) {
            if(mouseDragY > 1) {
                lr.enabled = false;
                mouseDown = false;
                rig.isKinematic = false;
                rig.velocity = new Vector3(mouseDragX / 50f, mouseDragY / 10f, mouseDragY / 12f);
                rig.AddTorque(new Vector3(-100f,0,0));
                airTime = 1f;
                lr.SetVertexCount(0);
                SFXManager.PlaySFX(3,isRightSide);

            }
            else {
                rig.isKinematic = true;
                transform.localPosition = startPosition;
                mouseDown = false;
            }
            txtFeed[1].color = Color.clear;
            
        }

        if(airTime > 0f) {
            if(!outBounds) {
                airTime -= Time.deltaTime;
            }
        }
        else if(rig.isKinematic == false) {
            ResetThrow();
            scoreScript.SwitchTurn();
        }
	}

    void FixedUpdate() {
        if(mouseDown) {
            float aux = 500f*Mathf.Abs(mouseStartPos.y - cam.ScreenToViewportPoint(InputPosition()).y);
            mouseDragX = 500f*(mouseStartPos.x - cam.ScreenToViewportPoint(InputPosition()).x);
            float convertAux = 0;
            if(mouseYCurrent != aux || mouseXCurrent != mouseDragX) {
                if(gravityForce.force.y < -1.1) {
                    if(Mathf.Abs(aux - 92.5f) < 15) {
                        convertAux = 92.5f + curve.Evaluate(Mathf.Abs(aux - 92.5f)/15f)*(aux - 92.5f);
                    }
                    else {
                        convertAux = aux;
                    }
                }
                else if(gravityForce.force.y < -1) {
                    if(Mathf.Abs(aux - 89.5f) < 15) {
                        convertAux = 89.5f + curve.Evaluate(Mathf.Abs(aux - 89.5f)/15f)*(aux - 89.5f);
                    }
                    else {
                        convertAux = aux;
                    }
                }
                else if(gravityForce.force.y < -0.9f) {
                    if(Mathf.Abs(aux - 87f) < 15) {
                        convertAux = 87f + curve.Evaluate(Mathf.Abs(aux - 87f)/15f)*(aux - 87f);
                    }
                    else {
                        convertAux = aux;
                    }
                }
                else if(gravityForce.force.y < -0.3) {
                    if(Mathf.Abs(aux - 53f) < 15) {
                        convertAux = 53f + curve.Evaluate(Mathf.Abs(aux - 53f)/15f)*(aux - 53f);
                    }
                    else {
                        convertAux = aux;
                    }
                }
                else if(gravityForce.force.y < -0.1f) {
                    if(Mathf.Abs(aux - 35f) < 15) {
                        convertAux = 35f + curve.Evaluate(Mathf.Abs(aux - 35f)/15f)*(aux - 35f);
                    }
                    else {
                        convertAux = aux;
                    }
                }

                if (convertAux < 100 && convertAux > 0) {
                    mouseDragY = convertAux;
                }
                else if(convertAux < 0) {
                    mouseDragY = 0;
                }
                else if(convertAux > 100) {
                    mouseDragY = 100;
                }
                if(txtFeed[1].color != Color.white) {
                    txtFeed[1].color = Color.white;
                }
                txtFeed[1].text = ((int)mouseDragY).ToString("0")+"%";
                angle = Mathf.Atan2(mouseDragY/10f,mouseDragY/12f) * Mathf.Rad2Deg;
                velocity = new Vector3(0,mouseDragY/10f,mouseDragY/12f).magnitude;
                lr.enabled = true;
                if(velocity > 0) {
                    RenderArc();
                }
                mouseYCurrent = convertAux;
                mouseXCurrent = mouseDragX;
            }
        }
    }

    public void ResetThrow() {
        rig.isKinematic = true;
        transform.localPosition = startPosition;
        mouseDown = false;
        outBounds = true;
        cameraMain.transform.localPosition = cameraPositions[0].vec3;
        cameraMain.transform.localEulerAngles = Quaternion.Euler(cameraPositions[0].quat.x,cameraPositions[0].quat.y,cameraMain.transform.localEulerAngles.z).eulerAngles;
    }

    public void StartBall() {
        if(!endRound) {
            endRound = true;
        }
    }

    public void StopBall() {
        if(endRound) {
            endRound = false;
            rig.isKinematic = true;
        }
    }

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.CompareTag("board")) {
            SFXManager.PlaySFX(5,isRightSide,coll.relativeVelocity.magnitude/10);
        }
        else if(coll.gameObject.CompareTag("steel")) {
            SFXManager.PlaySFX(2,isRightSide,coll.relativeVelocity.magnitude/20);
        }
        else {
            SFXManager.PlaySFX(1,isRightSide,coll.relativeVelocity.magnitude/10);
        }
    }

    void OnTriggerEnter(Collider coll) {
        if(coll.name == "closeCam") {
            cameraMain.transform.localPosition = cameraPositions[1].vec3;
            cameraMain.transform.localEulerAngles = Quaternion.Euler(cameraPositions[1].quat.x,cameraPositions[1].quat.y,cameraMain.transform.localEulerAngles.z).eulerAngles;
        }
    }

    void OnTriggerExit(Collider coll) {
        if(coll.name == "outOfBounds") {
            outBounds = false;
        }
    }

    void RenderArc() {
        lineRender.transform.localRotation = Quaternion.Euler(0,mouseDragX/7.3f,0);
        g = Mathf.Abs(gravityForce.force.y*10f);
        lr.SetVertexCount(resolution-1);
        lr.SetPositions(CalculateArcArray());
    }

    Vector3[] CalculateArcArray() {
        Vector3[] arcArray = new Vector3[resolution-1];
        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2*radianAngle)) / g;
        for (int i = 0; i < resolution-1; i++){
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }
        return arcArray;
    }

    Vector3 CalculateArcPoint(float t,float maxDistance) {
        float z = t * maxDistance;
        float y = z * Mathf.Tan(radianAngle) - ((g*z*z)/(2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(0,y,z);
    }

    public void SetGravity(float aux) {
        if(gravityForce != null) {
            gravityForce.force = new Vector3(0,aux/9.81f,0);
            txtFeed[0].text = Mathf.Abs(aux).ToString("0.000");
        }
        
    }

    bool InputGetDown() {
        if(isMobile) {
            bool aux = false;
            for (int i = 0; i < Input.touchCount; i++){
                if(Input.GetTouch(i).phase == TouchPhase.Began && myFingerID == -1){
                    if(cam.ScreenToViewportPoint(Input.GetTouch(i).position).x < 1f && cam.ScreenToViewportPoint(Input.GetTouch(i).position).x > 0f) {
                        myFingerID = Input.GetTouch(i).fingerId;
                        aux = true;
                    }
                }
            }
            return aux;
        }
        else {
            bool aux = false;
            if(Input.GetMouseButtonDown(0)) {
                if(cam.ScreenToViewportPoint(Input.mousePosition).x < 1f && cam.ScreenToViewportPoint(Input.mousePosition).x > 0f) {
                    aux = true;
                }
            }
            return aux;
        }
    }

    bool InputGetUp() {
        if(isMobile) {
            bool aux = false;
            for (int i = 0; i < Input.touchCount; i++){
                if(Input.GetTouch(i).phase == TouchPhase.Ended && Input.GetTouch(i).fingerId == myFingerID){
                    myFingerID = -1;
                    aux = true;
                }
            }
            return aux;
        }
        else {
            return Input.GetMouseButtonUp(0);
        }
    }

    Vector3 InputPosition() {
        if(isMobile) {
            Vector3 aux = Vector3.zero;
            for (int i = 0; i < Input.touchCount; i++){
                if(myFingerID == Input.GetTouch(i).fingerId && mouseDown) {
                    aux = Input.GetTouch(i).position;
                }
            }
            return aux;
        }
        else {
            return Input.mousePosition;
        }
    }

    public bool BallOnAir() {
        bool aux = true;
        if(scoreScript.PlayerTurn() == 1 && rig.isKinematic) { 
            aux = false;
        }
        return aux;
    }
}
