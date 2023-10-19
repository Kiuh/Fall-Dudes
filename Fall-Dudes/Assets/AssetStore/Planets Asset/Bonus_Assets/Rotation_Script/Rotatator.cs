using UnityEngine;

public class Rotatator : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotation;

    [SerializeField]
    private Transform meshObject = null;

    [SerializeField]
    private float rotationSpeed = 0;

    [SerializeField]
    private bool randomize;

    public bool Randomize => randomize;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float minSpeed;

    // Use this for initialization
    private void Start()
    {
        if (meshObject == null)
        {
            meshObject = transform.Find("planet");
            if (meshObject == null)
            {
                meshObject = transform.Find("w2");
            }
        }

        if (randomize)
        {
            rotation = new Vector3(RandFloat(), RandFloat(), RandFloat());
            rotationSpeed = Random.Range(minSpeed, maxSpeed);
        }
    }

    private float RandFloat()
    {
        return Random.Range(0f, 1.01f);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (meshObject != null)
        {
            meshObject.Rotate(rotation, rotationSpeed * Time.deltaTime);
        }
    }
}
