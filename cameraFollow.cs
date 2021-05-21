using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{

    public Transform target; //pega a posição do alvo que ela quer seguir
    public float smoothing; //suaviza a parada muito pouco fica devagar e muito fica rapido

    Vector3 offset; //Determina a distancia que a camera vai tentar manter do alvo
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position; //o objeto transform da CAMERA menos o objeto transforma do PERSONAGEM
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Código que faz a camera seguir no fixed update pq o personagem se move usando fixed update
    void FixedUpdate(){
        Vector3 targetCamPos = target.position + offset; //Basicamente a camera tenta chegar na posição do personagem mais o offset(setado no start)

        //Lerp --- basicamente de vez em quando se move de uma posição pra outra
        //Nesse caso sai da posição que está (transform.position), até a posição que a camera deveria estar em relação ao personagem
        //(targetCamPos) a cada certo tempo nesse caso smoothing * Time.deltaTime que vai praticamente dividir o smoothing e fazer a camera
        //ir suavemente até o alvo     
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime); 
    }
}
