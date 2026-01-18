using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using extOSC;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;

    // Whale boost
    public float whaleBoostAmount = 5f;
    public float whaleBoostDuration = 2f;
    private Coroutine whaleBoostCo;


    public AudioSource backgroundMusic;
    public AudioClip collectSound;
    public AudioClip deathSound;

    public TextMeshProUGUI countText;
    public TextMeshProUGUI winLooseText;
    public TextMeshProUGUI HighscoreText;

    public int oscPortNumber = 10000;
    public string oscDeviceUUID;

    private Rigidbody playerRigidbody;

    private float movementX;
    private float movementY;

    private AudioSource audioSource;

    private int count;
    private int maxCount;
    private int highscore;

    public SliderController sliderController;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // Initialize OSC
        OSCReceiver receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = oscPortNumber;
        receiver.Bind("/" + oscDeviceUUID + "/touch0", OnMoveOSC);

        count = 0;
        highscore = 0;
        //maxCount = GameObject.FindGameObjectsWithTag("Diamond").Length;

        highscore = PlayerPrefs.GetInt("highscore", 0);
        countText.text = "Trash collected: " + count;
        HighscoreText.text = "HIGHSCORE: " + highscore.ToString();

        winLooseText.gameObject.SetActive(false);
    }

    // FixedUpdate is called on a regular basis (see physics)
   


private void FixedUpdate()
{
    // Automatisches Gas geben (Z-Richtung)
    // Wert für 'vorwärts'
    float autoForwardSpeed = 1.0f; 

    // Bewegung berechnen
    // movementX kommt Steuerung 
    Vector3 targetVelocity = new Vector3(movementX * speed, playerRigidbody.linearVelocity.y, autoForwardSpeed * speed);

    
    // Prüft, ob der Rigidbody existiert und NICHT auf Kinematic steht
if (playerRigidbody != null && !playerRigidbody.isKinematic)
{
    // macht die Lenkung direkt
    playerRigidbody.linearVelocity = targetVelocity;
}
else 
{
    // Optional: Falls er Kinematic ist, bewegen wir ihn über die Position, 
    // damit er nicht stecken bleibt
    playerRigidbody.MovePosition(playerRigidbody.position + targetVelocity * Time.fixedDeltaTime);
}

// Keyboard
var keyboard = Keyboard.current;
if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
{
    BackToMenu();
}


       /* Vector3 movement = new Vector3(movementX, 0, movementY);

        playerRigidbody.AddForce(movement * speed);

        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        if (keyboard.escapeKey.wasPressedThisFrame)
        {
            BackToMenu();
        }
        */
    }

    // OnMove is called from ExtOSC
    public void OnMoveOSC(OSCMessage message)
    {
        movementX = (float)message.Values[0].DoubleValue;
        movementY = -(float)message.Values[1].DoubleValue;

        //Debug.Log("movementX = " + movementX.ToString("F6"));
        //Debug.Log("movementY = " + movementY.ToString("F6"));
    }

    // OnMove is called from the InputSystem
    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }
    public void ApplyWhaleBoost()
{
    if (whaleBoostCo != null) StopCoroutine(whaleBoostCo);
    whaleBoostCo = StartCoroutine(WhaleBoostRoutine());
}

private IEnumerator WhaleBoostRoutine()
{
    speed += whaleBoostAmount;
    yield return new WaitForSeconds(whaleBoostDuration);
    speed -= whaleBoostAmount;
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

            //if (count >= maxCount)
            //{
                //winLooseText.gameObject.SetActive(true);
                //winLooseText.text = "YEAPPPPEAAAAHHH!!";

                //Invoke(nameof(BackToMenu), 5f);
            //}
        }

        if (other.CompareTag("Enemy"))
        {
            
            sliderController.DecreaseProgress();

          if (sliderController.IsEmpty())
    {
        // DEIN bestehender Game-Over-Code
        backgroundMusic.Stop();
        audioSource.PlayOneShot(deathSound);

        playerRigidbody.isKinematic = true;

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
