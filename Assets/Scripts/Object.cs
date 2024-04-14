using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public Vector3 position;
    public Vector3 targetPosition;
    public float duration = 0.25f;
    public bool isMoving = false;


    public IEnumerator MoveCoroutine()
    {
        isMoving = true;
        position = targetPosition;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }

    public IEnumerator TryMoveCoroutine(Vector3 tryPosition)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration / 2)
        {
            transform.position = Vector3.Lerp(startPosition, tryPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(tryPosition, startPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;
        isMoving = false;
    }
}
