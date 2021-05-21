using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class garageDoorController : MonoBehaviour
{

    public bool resetable; //Resetar a porta no estado que ela voltou
    public GameObject door; //Porta
    public GameObject gear; //Engrenagem
    public bool startOpen; //Começar a abrir ou fechar

    bool firstTrigger = false; //Indica se ele ja foi usada alguma vez
    bool open = true; //Ela comecça aberta

    Animator doorAnim; //Animator da porta
    Animator gearAnim; //Animator da engrenagem
    AudioSource doorAudio; //AudioSource da portas

    // Start is called before the first frame update
    void Start()
    {
        doorAnim = door.GetComponent<Animator>(); //Consegue usar direto door. pq door é uma variavel que vai receber a porta
        gearAnim = gear.GetComponent<Animator>(); //Consegue usar direto gear. pq gear é uma variavel que vai receber a gear
        doorAudio = door.GetComponent<AudioSource>(); //Pega o AudioSource da porta

        if(!startOpen){
            open = false;
            doorAnim.SetTrigger("doorTrigger");
            doorAudio.Play();
        }
    }
    //Quando entra no collider do outro lado da porta
    void OnTriggerEnter(Collider other){
        if(other.tag == "Player" && !firstTrigger){
            if(!resetable){ //Se a porta não pode abrir e fechar entao
                firstTrigger = true; //isso é true e ela não pode mais abrir
            }
            doorAnim.SetTrigger("doorTrigger");
            open = !open;
            gearAnim.SetTrigger("gearRotating");
            doorAudio.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
