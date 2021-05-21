using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class safeHouseDoorController : MonoBehaviour
{
    public restartGame theGameController; //Vai ser usado pra instanciar o código de restart game

    public Text endGameText; //Texto

    AudioSource safeDoorAudio; //Variavel que guarda um audio source

    bool safe = false; //Pra deixar isso acontecer so uma vez


    // Start is called before the first frame update
    void Start()
    {
        safeDoorAudio = GetComponent<AudioSource>(); //Pega o audiosource que ta nele mesmo
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player" && safe == false){
            safe = true;
            Animator safeDoorAnim = GetComponentInChildren<Animator>(); //Pega o animator do filho
            safeDoorAnim.SetTrigger("safeHouseTrigger"); //Toca a animação
            safeDoorAudio.Play(); //toca o audio
            endGameText.text = "You win!";
            Animator endGameAnim = endGameText.GetComponent<Animator>(); //Pega o animator do engGameText
            endGameAnim.SetTrigger("endGame"); //Manda o trigger que toca animação de morte do player
            theGameController.restartTheGame();
        }
    }
}
