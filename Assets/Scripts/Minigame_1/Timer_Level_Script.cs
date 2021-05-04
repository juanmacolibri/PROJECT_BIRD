using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Para imprimir UI en pantalla
using System.IO;        //Para lectura de ficheros

public class Timer_Level_Script: MonoBehaviour
{
    //Varibles text que imprimiremos en pantalla
    public Text timerMinutes, timerSeconds, timerSeconds100;

    //Varibles para el reloj del juego
    public float startTime, stopTime;
    public float timerTime;
    private bool timeisRunning = false;

    //Enteros que guardaran la info del reloj en tiempo real
    public int minutesInt, secondsInt, seconds100Int;

    //Variables para lectura de ficheros
    public string [] txtLinesArray;
    private string myFilePath, fileName;

    public int[,] beatsArray; // Array 3D para almacenar los datos a comprarar con la pulsacion del jugador
                              // EJEMPLO DE RELLENO DEL ARRAY: 
                              // ---- PRIMER BEAT ----
                              // beatsArray[0, 0] --> Primer beat, minuto del beat 
                              // beatsArray[0, 1] --> Primer beat, segundo del beat 
                              // beatsArray[0, 2] --> Primer beat, centesima de segundo del beat
                              // ---- SEGUNDO BEAT ----
                              // beatsArray[1, 0] --> Segundo beat, minuto del beat 
                              // beatsArray[1, 1] --> Segundo beat, segundo del beat 
                              // beatsArray[1, 2] --> Segundo beat, centesima de segundo del beat
                              // ---- ETC ----


    //Variables para comprobar cuando pulsamos
    public int playerLastBeatMinutes, playerLastBeatSeconds, playerLastBeatSeconds100;

    //Variable donde guardaremos el valor exacto de la ultima pulsacion en centesimas de segundo.
    public int playerBeatInfoInSeconds100, playerBeatMargin; 

    public AudioSource MusicAudioSource; //Musica a reproducir


    // Start is called before the first frame update
    void Start(){

        //Declaracion del fechero a leer + ubicacion del fichero
        fileName = "BEATS_TIME_TABLE.txt";
        myFilePath = Application.dataPath + "/" + "Scripts" + "/" + "Minigame_1" + "/" + fileName;
        ReadFromTheFile(); //leemos el fichero

        TimerReset(); //Seteamos el tiempo a 0 reiniciando el reloj

        playerBeatMargin = 10; //Ponemos un margen de +-10 centesimas de segundo para cuando el player pulse el boton.

        MusicAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){

        //Obtencion de variables para representar minutos, segundos y milesimas
        timerTime = stopTime + (Time.time - startTime);
        minutesInt = (int)timerTime / 60;
        secondsInt = (int)timerTime % 60;
        seconds100Int = (int)(Mathf.Floor((timerTime - (secondsInt + minutesInt * 60)) * 100));

        if (timeisRunning){ //Para  mostrar el tiempo en el UI
            timerMinutes.text = minutesInt.ToString();
            timerSeconds.text = secondsInt.ToString();
            timerSeconds100.text = seconds100Int.ToString();
        }

        //Presionamos espacio
        if (Input.GetKeyDown("space")){
            
            if (!MusicAudioSource.isPlaying){ // La musica y el reloj empiezan a funcionar si no lo estaban
                MusicAudioSource.Play();
                TimerStart();
            }

            CheckPlayerBeat();
        }

    }


    public bool CheckPlayerBeat(){ //Funcion para comprobar si la pulsacion del jugador va al ritmo o no

        playerLastBeatMinutes    = minutesInt;    //Nos quedamos con el minuto en el que pulso el boton
        playerLastBeatSeconds    = secondsInt;    //Nos quedamos con el segundo en el que pulso el boton
        playerLastBeatSeconds100 = seconds100Int; //Nos quedamos con la centesima de segundo en el que pulso el boton

        playerBeatInfoInSeconds100 = (playerLastBeatMinutes * 60 *100) + (playerLastBeatSeconds * 100) + playerLastBeatSeconds100;

       Debug.Log("  ---PULSE:" + playerLastBeatMinutes + "-" + playerLastBeatSeconds + "-" + playerLastBeatSeconds100);

        //Bucle que recorre los datos guardados en el Array
        for (int i = 0; i < (beatsArray.Length/3); i++){
            //Comparamos si la pulsacion del jugador coincide (incluyendo el margen "playerBeatMargin")
            if ( ((beatsArray[i, 0] * 60 * 100) + (beatsArray[i, 1] * 100) + beatsArray[i, 2] - playerBeatMargin) <= playerBeatInfoInSeconds100 && ((beatsArray[i, 0] * 60 * 100) + (beatsArray[i, 1] * 100) + beatsArray[i, 2] + playerBeatMargin) >= playerBeatInfoInSeconds100 )
            {
                Debug.Log("BIEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEN");
            }


        }

        Debug.Log(minutesInt + " : " + secondsInt + " : " + seconds100Int + "   -   " + playerLastBeatMinutes + " : " + playerLastBeatSeconds + " : " + playerLastBeatSeconds100);
        return true;
    }

    public void TimerStart(){
        if (!timeisRunning){
            Debug.Log("Start");
            timeisRunning = true;
            startTime = Time.time;
        }
    }

    public void TimerStop(){
        if (timeisRunning){
            Debug.Log("Stop");
            timeisRunning = false;
            stopTime = Time.time;
        }
    }

    public void TimerReset(){
        Debug.Log("Reset");
        timerMinutes.text = timerSeconds.text = timerSeconds100.text = "00";
    }


    public void ReadFromTheFile(){ //Funcion de lectura de fichero TXT

        txtLinesArray = File.ReadAllLines(myFilePath);  //Leemos el fichero
        beatsArray = new int[txtLinesArray.Length, 3];  // [cantidad de beats que va a tener el nivel, 3] (1 beat --> Minuto, segundo, centesima de segundo)

        string[] txtLineSplit;
        char[] splitChar = { ' ' }; //Separador, en el TXT cada dato va separado de un espacio

        int i = 0;
        foreach (string line in txtLinesArray){ //Bucle para almacenar los datos de cada beat en un array
           
            txtLineSplit = line.Split(splitChar);

            int convertStringtoInt;
               
                int.TryParse(txtLineSplit[0], out convertStringtoInt);
                beatsArray[i, 0] = convertStringtoInt; //Minuto del beat

                int.TryParse(txtLineSplit[1], out convertStringtoInt);
                beatsArray[i, 1] = convertStringtoInt; //Segundo del beat

                int.TryParse(txtLineSplit[2], out convertStringtoInt);
                beatsArray[i, 2] = convertStringtoInt; //Milisegundo del beat
            i++;
        }

        //Debug.Log("beats: " + txtLinesArray.Length  +"       " +beatsArray[0, 0] + "   " + beatsArray[0, 1] + "   " + beatsArray[0, 2]);
    }


    //BACKUP DE FUNCION "CheckPlayerBeat" antiguo. --> NO FUNCIONA BIEN

   /* public bool CheckPlayerBeat()
    { //Funcion para comprobar si la pulsacion del jugador va al ritmo o no

        playerLastBeatMinutes = minutesInt;    //Nos quedamos con el minuto en el que pulso el boton
        playerLastBeatSeconds = secondsInt;    //Nos quedamos con el segundo en el que pulso el boton
        playerLastBeatSeconds100 = seconds100Int; //Nos quedamos con el milisegundo en el que pulso el boton
        Debug.Log(int.MaxValue + "  ---PULSE:" + playerLastBeatMinutes + "-" + playerLastBeatSeconds + "-" + playerLastBeatSeconds100);

        //Debug.Log(playerLastBeatMinutes - 1);
        for (int i = 0; i < (beatsArray.Length / 3); i++)
        {

            //COMPROBACION DE LA PULSACION DEL JUGADOR CON +-10ms de diferencia
            if (beatsArray[i, 1] <= (playerLastBeatSeconds) && beatsArray[i, 0] <= (playerLastBeatMinutes))
            {

                //CASO IDEAL --> El jugador lo clava
                if (playerLastBeatSeconds100 == beatsArray[i, 2])
                {

                    if (playerLastBeatSeconds == beatsArray[i, 1])
                    {

                        if (playerLastBeatSeconds100 == beatsArray[i, 0])
                        {

                            Debug.Log("BIEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEN");
                        }
                    }
                }


                //CASO 1 --> TODO OK --> 10 <= beatsArray[i, 2] <= 89  
                else if (beatsArray[i, 2] >= 10 && beatsArray[i, 2] <= 89)
                {

                    if (playerLastBeatSeconds100 >= (beatsArray[i, 2] - 10) && playerLastBeatSeconds100 <= (beatsArray[i, 2] + 10))
                    {

                        if (playerLastBeatSeconds == beatsArray[i, 1])
                        {

                            if (playerLastBeatMinutes == beatsArray[i, 0])
                            {

                                Debug.Log("BIEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEN");

                            }
                        }
                    }
                }

                //CASO 2 --> Redondeamos hacia arriba --> beatsArray[i, 2] >= 90
                else if (beatsArray[i, 2] >= 90)
                {

                    if (playerLastBeatSeconds100 >= (beatsArray[i, 2] - 10) && playerLastBeatSeconds100 <= (beatsArray[i, 2] + 10))
                    {

                        if (playerLastBeatSeconds == beatsArray[i, 1] || playerLastBeatSeconds == (beatsArray[i, 1] + 1))
                        { //Puede haber una diferencia de +1 en los segundos

                            if (playerLastBeatMinutes == beatsArray[i, 0] || playerLastBeatMinutes == (beatsArray[i, 0] + 1))
                            { //Puede haber una diferencia de +1 en los minutos

                                Debug.Log("BIEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEN");
                            }
                        }
                    }
                }


                //CASO 3 --> Redondeamos hacia abajo --> beatsArray[i, 2] <= 9
                else if (beatsArray[i, 2] <= 9)
                {

                    if (Mathf.Abs((playerLastBeatSeconds100 - 100) % 100) >= (beatsArray[i, 2] - 10) && Mathf.Abs((playerLastBeatSeconds100 - 100) % 100) <= (beatsArray[i, 2] + 10))
                    {

                        if (playerLastBeatSeconds == beatsArray[i, 1] || playerLastBeatSeconds == (beatsArray[i, 1] - 1))
                        { //Puede haber una diferencia de -1 en los segundos

                            if (playerLastBeatMinutes == beatsArray[i, 0] || playerLastBeatMinutes == (beatsArray[i, 0] - 1))
                            { //Puede haber una diferencia de -1 en los minutos

                                Debug.Log("BIEEEEEEEEEEEEEEEN" + "  ---BEATARRAY:  " + beatsArray[i, 0] + "-" + beatsArray[i, 1] + "-" + beatsArray[i, 2] + "  ---PULSE:" + playerLastBeatMinutes + "-" + playerLastBeatSeconds + "-" + playerLastBeatSeconds100);
                            }
                        }
                    }
                }


            }


        }


        //Debug.Log(minutesInt + " : " + secondsInt + " : " + seconds100Int + "   -   " + playerLastBeatMinutes + " " + playerLastBeatSeconds + " " + playerLastBeatSeconds100);
        return true;
    }*/


}
