using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class restartGame : MonoBehaviour
{

    public float restartTime; //Tempo que fica na tela de morte antes de restartar
    bool resetNow = false; //Se deve resetar agora
    float resetTime; //Tempo efetivo que é usado pra resetar que vai ser somado no restart time e comparado com o Time.time pra calcular em segundos




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(resetNow && resetTime <= Time.time){
            Application.LoadLevel(Application.loadedLevel); //Restarta a cena
        }
        if(Input.GetKey("escape")){
            quitGame(); //Basicamente Application.Quit();
        }
    }

    public void restartTheGame(){ //Como é publico pode ser pego por tudo
        resetNow = true; //Resete agora
        resetTime = restartTime + Time.time; //Converte o tempo em segundos, ja foi feito mil vezes, mesmo esquema de sempre
    }

    public void quitGame(){
        Application.Quit(); //Só fecha
    }
}
