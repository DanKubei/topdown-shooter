using System.Collections;
using System.Collections.Generic;
using GunKeyEvent = MainController.GunKeyEvent;
using UnityEngine;
using Utils;

public class CharacterController : MonoBehaviour
{
    public CameraController cameraController;
    public GunController gunController;
    public float speed, minAnimationAngle;
    public Interval[] anglesAnimationIntervals = new Interval[9];

    private Rigidbody _selfRigidbody;
    private Animator _selfAnimator;

    private const float diagonalSpeed = 0.707f;

    // Start is called before the first frame update
    private void Start()
    {
        _selfRigidbody = GetComponent<Rigidbody>();
        _selfAnimator = GetComponent<Animator>();
        cameraController.OnView += RotateToView;
    }

    private void RotateToView(RaycastHit hit)
    {
        float oldRotation = transform.rotation.eulerAngles.y;
        Vector3 direction = (transform.position - new Vector3(hit.point.x, hit.point.y, hit.point.z)).normalized;
        float rotation = (direction.z > 0 ? 180 : 0) + Mathf.Atan(direction.x / direction.z) * 180 / Mathf.PI;
        transform.rotation = Quaternion.Euler(0,
            rotation,
            0);
        float difference = oldRotation - rotation;
        print(difference);
        if (Mathf.Abs(difference) > minAnimationAngle)
        {
            _selfAnimator.SetInteger("TurnDirection", (int)Mathf.Sign(difference));
        }
        else
        {
            _selfAnimator.SetInteger("TurnDirection", 0);
        }
    }

    public void MoveOnKeyEvent(Vector2 movement)
    {
        if (movement == Vector2.zero)
        {
            _selfAnimator.SetInteger("RunDirection", 0);
            return;
        }
        float moveAngle = Vector2.Angle(Vector2.right, movement) * -Mathf.Sign(movement.y)
            + (transform.rotation.eulerAngles.y > 180 ? transform.rotation.eulerAngles.y - 360 : transform.rotation.eulerAngles.y);
        if (moveAngle > 180)
        {
            moveAngle = -360 + moveAngle;
        }
        if (moveAngle < -180)
        {
            moveAngle = 360 + moveAngle;
        }
        _selfRigidbody.velocity = Vector3.zero;
        _selfRigidbody.angularVelocity = Vector3.zero;
        if (movement.x != 0 && movement.y != 0)
        {
            _selfRigidbody.MovePosition(transform.position + new Vector3(movement.y, 0, movement.x) * speed * diagonalSpeed);
        }
        else
        {
            _selfRigidbody.MovePosition(transform.position + new Vector3(movement.y, 0, movement.x) * speed);
        }
        for (int i = 0; i < 8; i++)
        {
            if (i == 0)
            {
                if (anglesAnimationIntervals[0].Contains(moveAngle) || anglesAnimationIntervals[1].Contains(moveAngle))
                {
                    _selfAnimator.SetInteger("RunDirection", -4);
                }
            }
            else
            {
                int animationNumber = i - 4;
                if (i >= 4)
                {
                    animationNumber = i - 3;
                }
                if (anglesAnimationIntervals[i + 1].Contains(moveAngle))
                {
                    _selfAnimator.SetInteger("RunDirection", animationNumber);
                }
            }
        }
    }

    public void GunOnKeyEvent(GunKeyEvent gunKeyEvent)
    {
        if (gunKeyEvent == GunKeyEvent.Fire)
        {
            gunController.Fire();
        }
        if (gunKeyEvent == GunKeyEvent.Reload)
        {
            gunController.Reload();
        }
    }
}
