using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float sensitivityMultiplier;

    StarterAssetsInputs starterAssetsInput;

    private void Start()
    {
        starterAssetsInput = GetComponentInParent<StarterAssetsInputs>();
    }

    private void Update()
    {


        // get mouse input
        float mouseX = starterAssetsInput.look.x * sensitivityMultiplier;
        float mouseY = starterAssetsInput.look.y * sensitivityMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, speed * Time.deltaTime);
    }
}
