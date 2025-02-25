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
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            audiosource.PlayOneShot(wallimpact);

        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Instantiate(VFX_Explode, transform.position, Quaternion.identity);
            WinTextObject.gameObject.SetActive(true);
            WinTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            audiosource.PlayOneShot(youLose);

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
}
