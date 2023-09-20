using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Input")]
    [SerializeField] private float _maxMagnitude;
    private Vector2 _forceVector;
    private Vector2 _firstTouchPosition;

    [Header("Force")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _forceMultiplier;

    [Header("Trajectory")]
    [SerializeField] private LineRenderer _lr;

    [Header("Collision")]
    [SerializeField] private GameObject _collisionParticle;

    [Header("Dead")]
    [SerializeField] private SpriteRenderer _ballModel;
    [SerializeField] private GameObject _deadParticle;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        GatherInput();
        Trajectory();
    }

    private void GatherInput()
    {
        if (GameManager.Instance.GameOver)
        {
            _forceVector = Vector2.zero;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _firstTouchPosition = GetMousePosition();
        }

        if (Input.GetMouseButton(0))
        {
            _forceVector = GetMousePosition() - _firstTouchPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Force();

            _forceVector = Vector2.zero;
        }

        _forceVector = Vector3.ClampMagnitude(_forceVector, _maxMagnitude);

        static Vector2 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void Force()
    {
        _rb.AddForce(-_forceVector * _forceMultiplier);
    }

    private void Trajectory()
    {
        _lr.SetPosition(0, transform.position);
        _lr.SetPosition(1, (Vector2)transform.position - _forceVector);
    }

    private void Dead()
    {
        GameObject particleObject = Instantiate(_deadParticle, transform.position, Quaternion.identity);
        var psMain = particleObject.GetComponent<ParticleSystem>().main;
        psMain.startColor = _ballModel.color;

        GameManager.Instance.GameOverAction?.Invoke();
        AudioManager.Instance.BallSmash();

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>())
        {
            Instantiate(_collisionParticle, collision.contacts[0].point, Quaternion.identity);
            CameraController.Instance.Shake();
            AudioManager.Instance.PlayerBallCollision();
            //Debug.Log(collision.gameObject.GetComponent<Rigidbody2D>().mass);
        }

        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Dead();
        }
    }
}
