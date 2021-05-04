using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video_Script : MonoBehaviour
{

    private GameObject videoPlayer;
    private GameObject blackBackgroundVideo;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GameObject.Find("Intro_Sunlight_Studio");
        videoPlayer.GetComponent<VideoPlayer>().playOnAwake = true;

        blackBackgroundVideo = GameObject.Find("Black_Background");

        videoPlayer.GetComponent<VideoPlayer>().Play();

    }

    // Update is called once per frame
    void Update()
    {
        checkIfVideoIsOver();
    }

    void checkIfVideoIsOver(){ //Funcion que comprueba si el video ha terminado de reproducirse

        long playerCurrentFrame = videoPlayer.GetComponent<VideoPlayer>().frame; //Frame actual que se  esta reproduciendo
        long playerFrameCount = Convert.ToInt64(videoPlayer.GetComponent<VideoPlayer>().frameCount); //Cantidad global de frames
    

       //Debug.Log(playerCurrentFrame + " --- " + (playerFrameCount-1));

        if( playerCurrentFrame >= (playerFrameCount-1) ){ //Cuando lleguemos al ultimo frame, ocultamos el video
            Debug.Log("SE ACABO LO QUE SE DABA");
            videoPlayer.SetActive(false);
            blackBackgroundVideo.SetActive(false);

        }
    }
}
