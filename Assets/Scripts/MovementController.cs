using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float verticalThrust = 100f;
    [SerializeField] private float rotationThrust = 1f;
    [SerializeField] private ParticleSystem engineFlames;
    private Rigidbody rb;
    private AudioSource shipSounds;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shipSounds = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessRotation();
        ProcessThrust();
    }
    
    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(Vector3.up * verticalThrust * Time.deltaTime);
            Debug.Log("Thrusters activated");
            if (!engineFlames.isPlaying)
            {
                engineFlames.Play();
            }

            if (!shipSounds.isPlaying)
            {
                StartCoroutine(StartFade(shipSounds, 0.25f, 1));
                shipSounds.Play();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W))
        {
            engineFlames.Stop();
            StartCoroutine(StartFade(shipSounds, 0.25f, 0));
            //shipSounds.Stop();
        }
        
        
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            ApplyRotation(rotationThrust);
            Debug.Log("Left turn");
            //engineFlames.Play();
            
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            ApplyRotation(-rotationThrust);
            Debug.Log("Right turn");
           //engineFlames.Play();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
    
    IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        } 
        if(targetVolume == 0){shipSounds.Stop();}
        
    }
}
