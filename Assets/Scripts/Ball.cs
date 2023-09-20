using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;

    [Header("Mass")]
    [SerializeField] private GameObject _massDisplaySpriteHolder;
    [SerializeField] private SpriteRenderer[] _massDisplaySprites;
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _passiveColor;
    [SerializeField] private float[] _massValues;
    [SerializeField] private float _timeBtwMassEvolution;
    private float _massValueEvolutionTimer;
    private int _currentMassValueIndex;

    [Header("Dead")]
    [SerializeField] private SpriteRenderer _ballModel;
    [SerializeField] private GameObject _deadParticle;

    private void Update()
    {
        if (!_massDisplaySpriteHolder.activeSelf) return;

        if (_massValueEvolutionTimer >= _timeBtwMassEvolution)
        {
            _currentMassValueIndex++;

            if (_currentMassValueIndex >= _massDisplaySprites.Length) _currentMassValueIndex = 0;

            SetMassValueSprites();

            _massValueEvolutionTimer = 0;
        }
        else
        {
            _massValueEvolutionTimer += Time.deltaTime;
        }
    }

    private void SetMassValueSprites()
    {
        for (int i = 0; i < _massDisplaySprites.Length; i++)
        {
            _massDisplaySprites[i].color = i <= _currentMassValueIndex ? _activeColor : _passiveColor;
        }

        _rb.mass = _massValues[_currentMassValueIndex];
    }

    private void Dead()
    {
        GameObject particleObject = Instantiate(_deadParticle, transform.position, Quaternion.identity);
        var psMain = particleObject.GetComponent<ParticleSystem>().main;
        psMain.startColor = _ballModel.color;

        GameManager.Instance.PlayerDestroyedBallAction?.Invoke();
        AudioManager.Instance.BallSmash();

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Dead();
        }
    }
}
