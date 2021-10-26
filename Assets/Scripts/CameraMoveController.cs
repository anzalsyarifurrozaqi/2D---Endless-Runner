using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour {
    [Header("Position")]
    public Transform Player;
    public float HorizontalOffset;

    private void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = Player.position.x + HorizontalOffset;
        transform.position = newPosition;
    }
}
