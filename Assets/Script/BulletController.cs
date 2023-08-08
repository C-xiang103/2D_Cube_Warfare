using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 MoveDirection;
    [SerializeField] private float Speed = 1;
    private Vector3 moveDirection => MoveDirection;
    public Vector3 StartPoint;
    public float MaxLength = 10;

    private enum CubeType
    {
        Bullet,
        Enemy,
    }
    void FixedUpdate()
    {
        if((StartPoint - transform.position).magnitude > MaxLength)
            Destroy(gameObject);
        transform.position += Time.deltaTime * Speed * moveDirection;
    }

    public void LookAtMoveDirection()
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = targetRotation;
    }
}
