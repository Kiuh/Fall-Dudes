using UnityEngine;

public class StartZone : MonoBehaviour
{
    [SerializeField]
    private Transform firstPoint;

    [SerializeField]
    private Transform secondPoint;

    public Vector3 GetRandomPoint()
    {
        return new Vector3(
            Random.Range(firstPoint.position.x, secondPoint.position.x),
            Random.Range(firstPoint.position.y, secondPoint.position.y),
            Random.Range(firstPoint.position.z, secondPoint.position.z)
        );
    }
}
