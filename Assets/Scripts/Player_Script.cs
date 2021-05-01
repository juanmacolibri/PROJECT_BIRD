using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Script : MonoBehaviour
{
    private Vector2      position;
    //public Rigidbody2D  playerRigidbody2D;

    private Animator playerAnimator;                   //Empleado para controlar la animacion del personaje.

    // Start is called before the first frame update
    void Start(){

        //Inicializacion de los componentes
       // playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        

        if (Input.GetKeyDown("space")){ //TODO: Cambiar en el futuro a "Input.GetButtonDown" (Ver semana 4 Clases UASchool)

            playerAnimator.SetBool("Player_press_action_button", true);
        }
        else
        {
            playerAnimator.SetBool("Player_press_action_button", false);
        }




       
       // playerRigidbody2D.transform.position = new Vector3(position.x, position.y, 0);

    }
    
}
