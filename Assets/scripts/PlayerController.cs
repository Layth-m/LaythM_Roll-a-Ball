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
        }
    }
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            WinTextObject.gameObject.SetActive(true);
            WinTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";

        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText() ;
            audiosource.PlayOneShot(pickup);
        }
        

    }
}
