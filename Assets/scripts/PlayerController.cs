using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.0f;
    public TextMeshProUGUI countText;
    private Rigidbody rb;
    public GameObject WinTextObject;
    private float movementX;
    private float movementY;

    private int count;

    public AudioClip pickup;
    private AudioSource audiosource;
    public AudioClip youWin;
    public AudioClip youLose;
    public AudioClip wallimpact;


    public GameObject VFX_BURST;

    public GameObject VFX_Explode;

    private Vector3 targetPos;
    [SerializeField] private bool isMoving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        count = 0;
        rb = GetComponent<Rigidbody>();
        SetCountText();

        WinTextObject.SetActive(false);
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;

    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 12)
        {
            WinTextObject.SetActive(true);

            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            audiosource.PlayOneShot(youWin);
        }
    }
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);

        if (isMoving)
        {
            // Move the player towards the target position
            Vector3 direction = targetPos - rb.position;
            direction.Normalize();
            rb.AddForce(direction * speed);
        }
        if (Vector3.Distance(rb.position, targetPos) < 0.5f)
        {
            isMoving = false;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            audiosource.PlayOneShot(wallimpact);

        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
           
            Instantiate(VFX_Explode, transform.position, Quaternion.identity);
            Destroy(gameObject);
            WinTextObject.gameObject.SetActive(true);
            WinTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            audiosource.PlayOneShot(youLose);

            // Set the speed of the enemy's animation to 0
            collision.gameObject.GetComponentInChildren<Animator>().SetFloat("speed_f", 0);

        }

    }

  
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            Instantiate(VFX_BURST,transform.position, Quaternion.identity);
            SetCountText() ;
            audiosource.PlayOneShot(pickup);
        }
        

    }

    private void Update()
    {
        RaycastHit hit;
        if (Input.GetMouseButton(0)) // Check if left mouse button is held down
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    targetPos = hit.point; // Set target position
                    isMoving = true; // Start player movement
                }
            }
        }
        else
        {
            isMoving = false; // Stop player movement
        }

       
    }
}

