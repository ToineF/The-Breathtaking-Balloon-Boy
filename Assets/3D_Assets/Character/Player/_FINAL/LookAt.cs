using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class LookAtSetting
{
    public GameObject objEye;
    public Transform objPivotEye;
    public Transform objPivotLookAt;

    public float eyeXMin;
    public float eyeXMax;

    public float eyeYMin;
    public float eyeYMax;

}

public class LookAt : MonoBehaviour
{

    public List<LookAtSetting> Eyes;

    public string TextureShaderName;

    void Update()
    {
        foreach (LookAtSetting t in Eyes)
        {
            t.objPivotEye.LookAt(t.objPivotLookAt);

            Vector2 tempEyeRot = new Vector2(t.objPivotEye.localRotation.x, t.objPivotEye.localRotation.y);


            float tempEyeX_Limit = Mathf.Clamp(tempEyeRot.x, t.eyeXMin, t.eyeXMax);
            float tempEyeY_Limit = Mathf.Clamp(tempEyeRot.y, t.eyeYMin, t.eyeYMax);

            //t.objEye.GetComponent<Renderer>().material.SetTextureOffset(TextureShaderName, new Vector2(-tempEyeY_Limit, -tempEyeX_Limit));
        }
    }
}
