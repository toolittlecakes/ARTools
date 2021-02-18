using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMotion : MonoBehaviour
{
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        var newPosition = new Vector3(0, 0, Mathf.Sin(Time.time / 0.5f) * 0.5f);
        transform.localPosition = startPosition + newPosition;
    }
}
