using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour {
    
    [Header("Movement")]
    public float MoveAccel;
    public float MaxSpeed;

    [Header("Jumping")]
    public float JumpAccel;

    [Header("Ground Raycast")]
    public float GroundRayDistance;
    public LayerMask GroundLayerMask;

    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;

    [Header("GameOver")]
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")]
    public CameraMoveController gameCamera;

    private bool _isJump;
    private bool _isGround;
    private CharacterSoundController _soundController;
    private float _lastPositionX;

    Rigidbody2D RB;
    Animator Anim;

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        _soundController = GetComponent<CharacterSoundController>();

        _lastPositionX = transform.position.x;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isGround)
            {
                _isJump = true;
                _soundController.PlayJump();
            }
        }       

        Anim.SetBool("IsGround", _isGround);

        int distancePassed = Mathf.FloorToInt(transform.position.x - _lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

        if (scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            _lastPositionX += distancePassed;
        }

        if (transform.position.y < fallPositionY)
        {
            GameOver();
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, GroundRayDistance, GroundLayerMask);
        if (hit)
        {
            if (!_isGround && RB.velocity.y <= 0)
            {
                _isGround = true;
            }
        }
        else
        {
            _isGround = false;
        }


        Vector2 velocityVector = RB.velocity;

        if (_isJump)
        {
            velocityVector.y += JumpAccel;
            _isJump = false;
        }
        velocityVector.x = Mathf.Clamp(velocityVector.x + MoveAccel * Time.deltaTime, 0.0f, MaxSpeed);

        RB.velocity = velocityVector;
    }

    private void GameOver()
    {
        // set high score
        score.FinishScoring();

        // stop camera movement
        gameCamera.enabled = false;

        // show gameover
        gameOverScreen.SetActive(true);

        // disable this too
        this.enabled = false;
    }
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * GroundRayDistance), Color.white);
    }
}
