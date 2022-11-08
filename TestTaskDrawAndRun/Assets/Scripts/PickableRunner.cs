using UnityEngine;
using Extensions;
public class PickableRunner : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.HandleComponent<RunnerSpawner>((component) => component.SpawnAt(collision.transform.position));
        gameObject.SetActive(false);
    }
}
