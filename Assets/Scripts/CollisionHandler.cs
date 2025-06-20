using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float loadDelay = 2f;

    [Header("Audio")]
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip alarmSound;
    [SerializeField] private AudioClip beamSound;

    [Header("Lives and Scoring")]
    [SerializeField] private int lives = 3;
    [SerializeField] private int levelScoreGoal = 3;
    public int score = 0;

    [Header("Level Exit")]
    [SerializeField] ParticleSystem stationBeam;
    [SerializeField] private Transform stationPos;
    [SerializeField] private float speed = 1f;


    private Transform target;

    private AudioSource audioSource;

    private Rigidbody rb;

    PlatformController platformController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        platformController = FindFirstObjectByType<PlatformController>();
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Start":
                Debug.Log("Launch platform");
                break;

            case "Obstacle":
                Debug.Log("Obstacle hit");
                switch (lives)
                {
                    case > 1:
                        HitSequence();
                        break;

                    case 1:
                        audioSource.PlayOneShot(alarmSound);
                        HitSequence();
                        break;

                    case 0:
                        StartCrashSequence();
                        break;
                }
                break;

            case "Walls":
                Debug.Log("Reached bounds");
                break;

            default:
                Debug.Log("miscellaneous collision");
                break;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Finish":
                Debug.Log("Beamed to next level");
                StartSuccessSequence();
                break;

            case "Pickup":
                Debug.Log("Star picked up");
                PickupSequence();
                other.gameObject.SetActive(false);
                break;

            case "PowerUp":
                Debug.Log("Power up acquired");
                break;

            default:
                Debug.Log("Trigger detected");
                break;
        }
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    void StartSuccessSequence()
    {
        audioSource.PlayOneShot(successSound);
        GetComponent<MovementController>().enabled = false;
        Invoke(nameof(LoadNextLevel), loadDelay);
    }

    void StartCrashSequence()
    {
        audioSource.PlayOneShot(crashSound);
        score = 0;
        GetComponent<MovementController>().enabled = false;
        Invoke(nameof(ReloadLevel), loadDelay);
    }

    void PickupSequence()
    {
        audioSource.PlayOneShot(pickupSound);

        if (score < levelScoreGoal)
        {
            score++;
        }

        if (score >= levelScoreGoal)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;

            // move platform towards the player
            platformController.MoveToPlayer(transform);
        }
    }

    void HitSequence()
    {
        lives--;
        audioSource.PlayOneShot(hitSound);
    }
}
