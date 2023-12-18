using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    public Text ammoText;

    public void UpdateAmmoText(int ammo, int maxAmmo)
    {
        ammoText.text = $"{ammo}/{maxAmmo}";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
