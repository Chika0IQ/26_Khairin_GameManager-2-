using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{

    public GameObject addEnergyPrefab; // addEnergyCube gameobject
    public GameObject minusEnergyPrefab; // minusEnergyCube gameobject
    public Animator playerAnim; // Animator for player to call animation
    public Text Energy; // Display Energy in GameView

    public int numberOfSpawn; // Spawn how many cubes when collided

    float speed = 7f; // Speed of Player Movement
    bool canMove = true; // whether to start or stop movement
    float energyCount; // Count of how many energy is earned
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        SceneChange();
    }

    private void PlayerMovement()
    {
        if (canMove == true) // if canMove true, player will move else stop
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) // Forward movement with quaternion
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                StartRun();
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) // Left movement with quaternion
            {
                transform.rotation = Quaternion.Euler(0, 270, 0);
                StartRun();
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) // Backward movemnt with quaternion
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                StartRun();
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) // Right movement with quaternion
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
                StartRun();
            }

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
            {
                playerAnim.SetBool("isRun", false); // Player animation back to Idle
            }
        }

        if (GameManager.instance.levelTime == 0)
        {
            canMove = false; // Set bool canMove to false to stop player movement
            playerAnim.SetBool("isRun", false); // Player animation back to Idle
        }
    }

    private void StartRun() // Player animation to run and move forward, qauternion handle in movement function
    {
        playerAnim.SetBool("isRun", true);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AddEnergy"))
        {
            energyCount += 5; // add 5 to totalenergyCount

            //Debug.Log("Total: " + energyCount);

            GameManager.instance.levelTime += 5f;
            Energy.GetComponent<Text>().text = "Energy: " + energyCount;
            Destroy(collision.gameObject); // Destroy current collided instance
            addEnergyCube(); // addEnergyCube function
           
        }
        else if (collision.gameObject.CompareTag("MinusEnergy"))
        {
            energyCount -= 25;// subtract 25 from totalenergyCount

            //Debug.Log("Total: " + energyCount);

            GameManager.instance.levelTime -= 5f;
            Energy.GetComponent<Text>().text = "Energy: " + energyCount;
            Destroy(collision.gameObject); // Destroy current collided instance
            addMinusCube(); // minusEnergyCube function
        }
    }

    private void addEnergyCube() // Spawn 4 more addEnergyCubes
    {
        for (int i = 0; i < numberOfSpawn; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-15, 15), 0, Random.Range(-15, 15));

            if (Random.Range(0, 1) < 1)
            {
                Instantiate(addEnergyPrefab, randomPos, Quaternion.identity);
            }
        }
    }

    private void addMinusCube() // Spawn 4 more minusEnergyCubes
    {
        for (int i = 0; i < numberOfSpawn; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-15, 15), 0, Random.Range(-15, 15));

            if (Random.Range(0, 1) < 1)
            {
                Instantiate(minusEnergyPrefab, randomPos, Quaternion.identity);
            }
        }
    }

    private void SceneChange() // Change of scene if conditions meet
    {
        if (GameManager.instance.levelTime <= 0)
        {
            SceneManager.LoadScene("LoseScene");
        }
        if (energyCount >= 50f)
        {
            SceneManager.LoadScene("WinScene");
        }
    }
}
