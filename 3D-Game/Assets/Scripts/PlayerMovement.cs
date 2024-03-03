using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    public AudioClip walkingSound; // Assign the "Walking" audio clip in the Unity Editor
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Play "Walking" audio when the player is moving
        if (move.magnitude > 0 && !audioSource.isPlaying)
        {
            audioSource.clip = walkingSound;
            audioSource.Play();
        }
        // Stop playing "Walking" audio when the player stops moving
        else if (move.magnitude == 0 && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
