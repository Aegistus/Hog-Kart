using UnityEngine;

public class ParallelParkingPad : MonoBehaviour
{
    [SerializeField] Transform referenceTransform;
    [SerializeField] float timeToDisappear = 2f;
    [SerializeField] float angleAllowance = 10f;
    [SerializeField] float minimumDistance = 5f;
    [SerializeField] Material redMaterial;
    [SerializeField] Material yellowMaterial;
    [SerializeField] Material greenMaterial;
    [SerializeField] MeshRenderer visual;

    Transform player;
    float chargeTimer = 0f;
    bool charging = false;

    private void Start()
    {
        player = FindAnyObjectByType<CarController>().transform;
    }

    private void Update()
    {
        if (chargeTimer >= timeToDisappear)
        {
            visual.gameObject.SetActive(false);
            return;
        }

        if (Vector3.Distance(player.position, transform.position) < minimumDistance && Vector3.Angle(player.forward, referenceTransform.forward) < angleAllowance)
        {
            chargeTimer += Time.deltaTime;
            if (!charging)
            {
                visual.material = yellowMaterial;
                charging = true;
            }
        }
        else
        {
            if (charging)
            {
                visual.material = redMaterial;
                chargeTimer = 0;
                charging = false;
            }
        }
    }
}
