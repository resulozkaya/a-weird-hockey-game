using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] private AudioSource _effectSource;

    [Header("Clips")]
    [SerializeField] private AudioClip _playerBallCollision;
    [SerializeField] private AudioClip _ballSmash;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    public void PlayerBallCollision()
    {
        _effectSource.volume = 1;
        _effectSource.PlayOneShot(_playerBallCollision);
    }

    public void BallSmash()
    {
        _effectSource.volume = .5f;
        _effectSource.PlayOneShot(_ballSmash);
    }
}
