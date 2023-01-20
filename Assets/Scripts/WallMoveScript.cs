using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMoveScript : MonoBehaviour
{
    [SerializeField] Vector3 first;
    [SerializeField] Vector3 second;
    [SerializeField] float duration;

    IEnumerator Start()
    {
        transform.position = first;

        while (true)
        {
            yield return StartCoroutine(MoveWall(first, second));
            yield return StartCoroutine(MoveWall(second, first));
        }
    }

    IEnumerator MoveWall(Vector3 A, Vector3 B)
    {
        float time = 0.0f;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(A, B, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
