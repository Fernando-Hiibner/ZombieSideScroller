using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootFireball : MonoBehaviour
{

    public float damage; //Dano
    public float speed; //Velocidade que o projetil vai se mover

    Rigidbody myRB; //Pega o Rigidbody

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponentInParent<Rigidbody>(); //Pega o componente no pai
        if(transform.rotation.y > 0){
            myRB.AddForce(Vector3.right*speed, ForceMode.Impulse); //Vai pra direita 
        }else{
            myRB.AddForce(Vector3.right*-speed, ForceMode.Impulse); //Atira pra direita mas agora rotacionado pro lado certo
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        //Nesse caso vai pegar o gameObject do collider que ele colidiu e pegar o layer desse gameObject
        if(other.tag == "Enemy" || other.gameObject.layer == LayerMask.NameToLayer("Shootable")){
            myRB.velocity = Vector3.zero; //Para de mover tudo
            //Procura o script de vida do inimigo
            enemyHealth theEnemyHealth = other.GetComponent<enemyHealth>();
            //Se encontrar:
            if(theEnemyHealth != null){
                theEnemyHealth.addDamage(damage); //Primeiro da o dano em si
                theEnemyHealth.addFire(); //Chama função pra dar dano de fogo no inimigo
            }
            Destroy(gameObject);
        }
    }
}
