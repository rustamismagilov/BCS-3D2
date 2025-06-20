using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private ParticleSystem beamParticles;

    private Transform targetPlayer;
    private bool shouldMove = false;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    void Update()
    {
        if (!shouldMove || targetPlayer == null) return;

        Vector3 targetPosition = targetPlayer.position + offset;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < Mathf.Epsilon)
        {
            beamParticles.Play();
            shouldMove = false;
        }
    }

    public void MoveToPlayer(Transform player)
    {
        targetPlayer = player;
        meshRenderer.enabled = true;
        shouldMove = true;
    }
}
