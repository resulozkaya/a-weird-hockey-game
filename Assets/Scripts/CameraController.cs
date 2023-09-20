using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [Header("Shake")]
    [SerializeField] private float _shakeDuration;
    [SerializeField] private float _shakeMagnitude;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shake();
        }
    }

    public void Shake()
    {
        transform.DOShakePosition(_shakeDuration, _shakeMagnitude);
    }
}
