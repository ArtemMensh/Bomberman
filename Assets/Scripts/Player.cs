using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    [SerializeField] float speedPlayer = 6; // Speed Player
    float gravityForce; // Gravity Player
    Vector3 moveVector; // Vector move Player

    [SerializeField] GameObject prefabBomb;

    CharacterController characterController;

    enum StatePlayer
    {
        life,
        die
    };

    StatePlayer statePlayer = StatePlayer.life;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (statePlayer == StatePlayer.life)
        {
            MovePlayer();
            Gravity();
            PlantBomb();
        }
    }

   public void Die(){
        statePlayer = StatePlayer.die;
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void PlantBomb()
    {
        if (Input.GetButtonUp("Jump"))
        {
            Vector3 coordinateBobmb = AroundCoordinateBomb();

            GameObject bomb = Instantiate(prefabBomb, coordinateBobmb, Quaternion.identity);

        }
    }

    Vector3 AroundCoordinateBomb()
    {
        Vector3 coordinateBobmb = new Vector3(0, 0, 0);

        coordinateBobmb.x = Mathf.Round(transform.position.x);
        coordinateBobmb.y = transform.position.y;
        coordinateBobmb.z = Mathf.Round(transform.position.z);

        return coordinateBobmb;
    }

    void MovePlayer()
    {
        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * speedPlayer;
        moveVector.z = Input.GetAxis("Vertical") * speedPlayer;
        moveVector.y = gravityForce * Time.deltaTime;

        characterController.Move(moveVector * Time.deltaTime);
    }

    void Gravity()
    {
        if (!characterController.isGrounded)
        {
            gravityForce -= 20f;
        }
        else
        {
            gravityForce = -1f;
        }
    }
}
