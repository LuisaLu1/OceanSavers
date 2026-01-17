using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using extOSC;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;

    public AudioSource backgroundMusic;
    public AudioClip collectSound;
    public AudioClip deathSound;

    public TextMeshProUGUI countText;
    public TextMeshProUGUI winLooseText;

    public int oscPortNumber = 10000;
    public string oscDeviceUUID;

    private Rigidbody playerRigidbody;

    private float movementX;
    private float movementY;

    private AudioSource audioSource;

    private int count;
    private int maxCount;

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
        maxCount = GameObject.FindGameObjectsWithTag("Diamond").Length;

        countText.text = "Collected " + count + " of " + maxCount;

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

    
    // macht die Lenkung direkt
    playerRigidbody.linearVelocity = targetVelocity;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Diamond"))
        {
            audioSource.PlayOneShot(collectSound);

            other.gameObject.SetActive(false);

            count++;

            countText.text = "Collected " + count + " of " + maxCount;

            if (count >= maxCount)
            {
                winLooseText.gameObject.SetActive(true);
                winLooseText.text = "YEAPPPPEAAAAHHH!!";

                Invoke(nameof(BackToMenu), 5f);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            backgroundMusic.Stop();
            audioSource.PlayOneShot(deathSound);

            playerRigidbody.isKinematic = true;

            winLooseText.gameObject.SetActive(true);
            winLooseText.text = "GAME OVER!!";

            Invoke(nameof(BackToMenu), 5f);
        }
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
