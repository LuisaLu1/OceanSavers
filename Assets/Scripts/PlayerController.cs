

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

// We removed extOSC and replaced it with OscCore logic for better performance
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5.0f;
    public float steeringSensitivity = 15f;
    public float horizontalLimit = 12f;
    private float baseSpeed;

    [Header("Whale Boost")]
    public float whaleBoostAmount = 2f;
    public float whaleBoostDuration = 2f;
    private Coroutine whaleBoostCo;

    [Header("Audio")]
    public AudioSource backgroundMusic;
    public AudioClip collectSound;
    public AudioClip deathSound;

    [Header("UI References")]
    public TextMeshProUGUI countText;
    public TextMeshProUGUI winLooseText;
    public TextMeshProUGUI HighscoreText;
    public SliderController sliderController;

    private Rigidbody playerRigidbody;
    private AudioSource audioSource;

    private float movementX; // This is now controlled by OSC Z-tilt
    private int count;
    private int highscore;
    private Vector3 gravityValue;

    private void Start()
    {
        baseSpeed = speed;
        playerRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // We use isKinematic to prevent 'linear velocity' errors while moving via code
        playerRigidbody.isKinematic = true;

        count = 0;
        highscore = PlayerPrefs.GetInt("highscore", 0);
        
        countText.text = "Trash collected: " + count;
        HighscoreText.text = "HIGHSCORE: " + highscore.ToString();
        winLooseText.gameObject.SetActive(false);
    }

    // This is the function called by the OscCore Vector3 Input component
    public void OnMoveOSC(Vector3 gravity)
    {
        gravityValue = gravity;
        // Since Z is your most active axis on the phone, we use it for steering
        movementX = -gravityValue.z; 
    }

    private void FixedUpdate()
    {
        // 1. Calculate Forward Movement (Z)
        float autoForwardSpeed = 1.0f;
        float nextZ = transform.position.z + (autoForwardSpeed * speed * Time.fixedDeltaTime);

        // 2. Calculate Steering (X)
        float nextX = transform.position.x + (movementX * steeringSensitivity * Time.fixedDeltaTime);
        
        // 3. Keep player inside the river bounds
        nextX = Mathf.Clamp(nextX, -horizontalLimit, horizontalLimit);

        // 4. Apply Position to Kinematic Rigidbody
        // We use MovePosition for smooth movement without physics jitter
        Vector3 newPosition = new Vector3(nextX, transform.position.y, nextZ);
        playerRigidbody.MovePosition(newPosition);

        // Keyboard handling
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            BackToMenu();
        }
    }

    public void ApplyWhaleBoost()
    {
        if (whaleBoostCo != null) StopCoroutine(whaleBoostCo);
        whaleBoostCo = StartCoroutine(WhaleBoostRoutine());
    }

    private IEnumerator WhaleBoostRoutine()
    {
        speed = baseSpeed * whaleBoostAmount;
        yield return new WaitForSeconds(whaleBoostDuration);
        speed = baseSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Diamond"))
        {
            audioSource.PlayOneShot(collectSound);
            other.gameObject.SetActive(false);
            count++;
            countText.text = "Collected trash: " + count;

            if (highscore < count)
            {
                highscore = count;
                PlayerPrefs.SetInt("highscore", highscore);
                PlayerPrefs.Save();
                HighscoreText.text = "HIGHSCORE: " + highscore;
            }
        }

        if (other.CompareTag("Enemy"))
        {
            sliderController.DecreaseProgress();

            if (sliderController.IsEmpty())
            {
                backgroundMusic.Stop();
                audioSource.PlayOneShot(deathSound);
                
                // Stop movement
                speed = 0;
                steeringSensitivity = 0;

                winLooseText.gameObject.SetActive(true);
                winLooseText.text = "GAME OVER!!";
                Invoke(nameof(BackToMenu), 5f);
            }
        }
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}