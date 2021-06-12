using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPattern : MonoBehaviour, IProjectilePattern
{
    public Vector3[] GetProjectiles()
    {
        return new Vector3[] { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 0, -1) };
    }
}
