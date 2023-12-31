using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UI_InGameMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPanel;


    private StarterAssetsInputs starterAssetsInputs;
    private PlayerInput playerInput;

    private bool isPaused = false;

    private Transform player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        playerInput = player.GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
        menuPanel.SetActive(false);     
    }

    private void Update()
    {
        if (starterAssetsInputs.esc && !isPaused)
        {
            PauseGame();
            starterAssetsInputs.esc = false;
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        menuPanel.SetActive(true);
        playerInput.enabled = false;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void ContinueGame()
    {
        Time.timeScale = 1;
        isPaused= false;    
        playerInput.enabled = true;
        menuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
