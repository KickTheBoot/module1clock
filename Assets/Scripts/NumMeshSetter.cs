using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumMeshSetter : MonoBehaviour
{   public int number;
    public NumMesh Target;

    void OnValidate() {
        if(Target) Target.number = number;
    }
}
