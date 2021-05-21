using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootBullet : MonoBehaviour
{

    public float range = 10f; //Alcance do tiro
    public float damage = 5f; //Dano do tiro

    Ray shootRay; //Vai criar um ray - raio
    RaycastHit shootHit; //Informação que retorna quando um ray acerta algo
    int shootableMask; //Vai guardar onde esta o layer shootable pra poder acertar apenas o que é acertavel
    LineRenderer gunLine; //Instancia uma variavel que pode guardar um LineRenderer

    // Use this for initialization
    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable"); //Procura qual posição esta o layer shootable
        gunLine = GetComponent<LineRenderer>(); //Instancia o LineRenderer do bullet

        shootRay.origin = transform.position; //Instancia o raio do tiro a partir da onde ele está sendo chamado
        shootRay.direction = transform.forward; //Vai castar o raio na direção do transform do objeto que esta chamando (gunMuzzle)
        gunLine.SetPosition(0,transform.position); //Vai iniciar na posição de onde o tiro foi instanciado (gunMuzzle)

        //Vai pegar a informação shootRay e vai retornar a informação dele para o shootHit, a distancia (range) que podera ser atirado
        //e vai "acertar" apenas o shootableMask
        if(Physics.Raycast(shootRay, out shootHit, range, shootableMask)){
            //Vai usar essa informação do shoothit e ver se o collider que ele acertou tem tag "Enemy"
            if(shootHit.collider.tag == "Enemy"){
                //Se tiver ele instancia uma var do tipo enemyHealth que pega o componente EnemyHealt(script)
                //Do objeto que o ray acertou, nesse caso assumi-se que esta no top layer
                //Se não estiver teria que pesquisar no root do gameObject
                enemyHealth theEnemyHealth = shootHit.collider.GetComponent<enemyHealth>();
                //Se ele encontrar o script:
                if(theEnemyHealth != null){
                    theEnemyHealth.addDamage(damage); //Chama a função dentro do inimigo que da dano nele
                    //Vai instancia o efetio de dano no ponto onde o shootHit acertou e na direção oposta do raio em si
                    //Pra ficar mais claro, revisa o script Enemy health e ve os parametros da função DamageFX
                    theEnemyHealth.damageFX(shootHit.point, -shootRay.direction);

                }
            }
            gunLine.SetPosition(1,shootHit.point); // Vai vir da posição inicial até aqui(a partir da arma até o alvo), shootHit.point no caso
        }else{
            gunLine.SetPosition(1,shootRay.origin+shootRay.direction*range); //Se não acertar nada ele vai atirar um raio até o alcance maximo que ele consegue
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
