using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationResetter : MonoBehaviour
{
    [SerializeField] private RectTransform _transform;

    // Update is called once per frame
    void Update()
    {
        _transform.rotation = Quaternion.identity;
    }
}
