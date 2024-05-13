using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{

    public GameObject objEyeL;
    public GameObject objEyeR;

    private Renderer renderEyeL, renderEyeR;

    public Transform objPivotEye;
    public Transform objPivotLookAt;

    // Start is called before the first frame update
    void Start()
    {
        renderEyeL = objEyeL.GetComponent<Renderer>();
        renderEyeR = objEyeR.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        objPivotEye.LookAt(objPivotLookAt);

        Vector2 tempEyeRot = new Vector2(objPivotEye.localRotation.x, objPivotEye.localRotation.y);

        renderEyeL.material.SetTextureOffset("M_Eyes", tempEyeRot);
        renderEyeR.material.SetTextureOffset("M_Eyes", tempEyeRot);
    }
}
