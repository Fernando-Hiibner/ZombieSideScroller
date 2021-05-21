using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevatorController : MonoBehaviour
{

    public float resetTime; //Tempo que o elevador espera até descer dnvo
    Animator elevAnim; //Animator do elevador
    AudioSource elevAS; //AudioSource do elevaodr

    float downTime;

    bool elevIsUp = false; //Esta no alto?

    // Start is called before the first frame update
    void Start()
    {
        elevAnim = GetComponent<Animator>(); //Pega o Animator
        elevAS = GetComponent<AudioSource>(); //Pega o AudioSource
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            Rigidbody playerRB = other.GetComponent<Rigidbody>(); //Rigid do player
            playerRB.velocity = Vector3.zero;
            elevAnim.SetTrigger("activateElevator"); //Começa a animação de subida
            downTime = Time.time + resetTime; //Seta o tempo em segundos que ele vai demorar pra subir
            //Exemplo --- Time.time = 30 segundos + resetTime = 10s logo downTime = 40s
            //If do Update então checa a cada frame se o Tempo atual de execução passou esse downTime
            elevIsUp = true; //Pq ele subiu
            elevAS.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Se o Time.time passou o down time e o Elevador esta elevado
        if(downTime <= Time.time && elevIsUp){
            elevAnim.SetTrigger("activateElevator"); //Desce o elevador
            elevIsUp = false; //Pq ele desceu
            elevAS.Play();
        }
    }
}
