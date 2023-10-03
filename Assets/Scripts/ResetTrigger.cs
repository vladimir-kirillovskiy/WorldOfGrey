using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private Vector3 initPosition;

    private void Start()
    {
        initPosition = player.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        Debug.Log("Teleport");
        player.position = initPosition;
    }
}
