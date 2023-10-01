using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmViewPosition : MonoBehaviour
{
    private Vector3 center;
    [SerializeField]
    private float width;
    private void Awake()
    {
        center = transform.position;
    }
    public void UpdateTransform(float ratio)
    {
        transform.position = new Vector3(center.x + (width * ratio) - width / 2, center.y, center.z);
    }
}
