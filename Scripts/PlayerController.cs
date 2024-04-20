using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerNumber { Player1, Player2 }
    public PlayerNumber playerNumber;
    public float speed = 3;
    public float rotationSpeed = 90;
    public float gravity = -20f;
    public float jumpSpeed = 15;

    private KeyCode moveForwardKey;
    private KeyCode moveBackwardKey;
    private KeyCode rotateLeftKey;
    private KeyCode rotateRightKey;
    private KeyCode jumpKey;

    private CharacterController characterController;
    private Vector3 moveVelocity;
    private Vector3 turnVelocity;

    private int player1Score = 0;
    private int player2Score = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI timerText;

    private float timer = 0f;
    private bool timerStarted = false;
    private bool timerStopped = false;

    private bool finishLineCrossed = false;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (playerNumber == PlayerNumber.Player1)
        {
            moveForwardKey = KeyCode.W;
            rotateLeftKey = KeyCode.A;
            rotateRightKey = KeyCode.D;
            jumpKey = KeyCode.Q;
        }
        else if (playerNumber == PlayerNumber.Player2)
        {
            moveForwardKey = KeyCode.I;
            moveBackwardKey = KeyCode.K;
            rotateLeftKey = KeyCode.J;
            rotateRightKey = KeyCode.L;
            jumpKey = KeyCode.O;
        }
    }

    void Update()
    {
        if (timerStarted && !timerStopped)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }

        var hInput = 0f;
        var vInput = 0f;

        if (playerNumber == PlayerNumber.Player1)
        {
            hInput = Input.GetAxis("Horizontal");
            vInput = Input.GetAxis("Vertical");
        }
        else if (playerNumber == PlayerNumber.Player2)
        {
            hInput = Input.GetAxis("Horizontal2");
            vInput = Input.GetAxis("Vertical2");
        }

        if (characterController.isGrounded)
        {
            moveVelocity = transform.forward * speed * vInput;
            turnVelocity = transform.up * rotationSpeed * hInput;
            if (Input.GetKeyDown(jumpKey))
            {
                moveVelocity.y = jumpSpeed;
            }
        }
        moveVelocity.y += gravity * Time.deltaTime;
        characterController.Move(moveVelocity * Time.deltaTime);
        transform.Rotate(turnVelocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            if (playerNumber == PlayerNumber.Player1)
            {
                player1Score++;
                scoreText.text = "Player 1 Checkpoint: " + player1Score;
            }
            else if (playerNumber == PlayerNumber.Player2)
            {
                player2Score++;
                scoreText.text = "Player 2 Checkpoint: " + player2Score;
            }

            if (player1Score == 4 || player2Score == 4) // Check if it's the third checkpoint
            {
                timerStopped = true; // Stop the timer
            }

            if (!timerStarted)
            {
                timerStarted = true;
            }
        }
        else if (other.CompareTag("FinishLine"))
        {
            if (!finishLineCrossed)
            {
                finishLineCrossed = true;
                if (playerNumber == PlayerNumber.Player1)
                {
                    winText.text = "Player 1 Wins!";
                }
                else if (playerNumber == PlayerNumber.Player2)
                {
                    winText.text = "Player 2 Wins!";
                }
            }
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
