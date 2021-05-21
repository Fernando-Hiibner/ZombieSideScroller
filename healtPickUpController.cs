using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healtPickUpController : MonoBehaviour
{

    public float healthAmount; //Vida ele recupera
    public AudioClip healtPickUpSound; //Seta o audio pelo código pq se o objeto for destruido ele ainda toca

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Quando o player entrar em contato com o coração
    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            other.GetComponent<playerHealth>().addHealth(healthAmount); //Pega o script do player e adciona vida
            Destroy(transform.root.gameObject); //Destroi o pai desse objeto(Junto com ele mesmo)
            //Vai tocar algo na posição do segundo paramentro
            //Parametros --- AudioClip que vai tocar, posição onde vai tocar, volume em float
            AudioSource.PlayClipAtPoint(healtPickUpSound, transform.position, 0.2f);
        }
    }
}
