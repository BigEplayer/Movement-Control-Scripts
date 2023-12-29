using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  maybe make singleton
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;



    private void LateUpdate()
    {
        Track();
    }

    private void Track()
    {
        var targetPosition = new Vector3(target.position.x + xOffset, target.position.y + yOffset, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
    }
}
