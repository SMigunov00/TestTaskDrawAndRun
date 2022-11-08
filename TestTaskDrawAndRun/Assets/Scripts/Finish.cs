using UnityEngine;
using Extensions;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.HandleComponent<GameState>((component) => component.CrossingFinishLine());
    }
}
