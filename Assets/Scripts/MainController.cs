using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public enum GunKeyEvent 
    {
        Fire,
        Reload
    }

    public CharacterController characterController;
    public CameraController cameraController;
    public UIController UIController;
    public GunScriptableObject currentGun;

    private GunController gunController;

    // Start is called before the first frame update
    private void Start()
    {
        gunController = characterController.gunController;
        gunController.ChangeParameters(currentGun);
    }

    // Update is called once per frame
    private void Update()
    {
        GetKeyEvents();
    }

    private void GetKeyEvents()
    {
        GetMovementKeyEvents();
        GetGunKeyEvents();
    }

    private void GetMovementKeyEvents()
    {
        Vector2 movement = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movement.x = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement.y = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movement.y = -1;
        }
        characterController.MoveOnKeyEvent(movement);
    }

    private void GetGunKeyEvents()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            characterController.GunOnKeyEvent(GunKeyEvent.Fire);
        }
        else if (Input.GetKey(KeyCode.R))
        {
            characterController.GunOnKeyEvent(GunKeyEvent.Reload);
        }
        else 
        {
            return;
        }
        UIController.gameLayout.UpdateAmmoText(gunController.Ammo, currentGun.maxAmmo);
    }
}
