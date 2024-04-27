using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameState
{
    public TransformSerializable cameraTransform;
    public TransformSerializable playerTransform;
    public TransformSerializable[] enemiesTransform;
    public float timeLeft;
    public int attempt;
}

[Serializable]
public class TransformSerializable
{
    public float[] variables;

    
    public static TransformSerializable GetTransSer(Transform source)
    {
        var res = new TransformSerializable()
        {
            variables = new float[10] {
                source.position.x,
                source.position.y,
                source.position.z,
                source.rotation.x,
                source.rotation.y,
                source.rotation.z,
                source.rotation.w,
                source.localScale.x,
                source.localScale.y,
                source.localScale.z
            }
        };
        return res;
    }

    public static void SetTransform(Transform target, TransformSerializable source)
    {
        target.position = new Vector3(source.variables[0], source.variables[1], source.variables[2]);
        target.rotation = new Quaternion(source.variables[3], source.variables[4], source.variables[5], source.variables[6]);
        target.localScale = new Vector3(source.variables[7], source.variables[8], source.variables[9]);

    }

}