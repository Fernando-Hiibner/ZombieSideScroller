using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeScript : MonoBehaviour
{

    public float damage; //Dano do melee
    public float knockBack; //O Brabo
    public float knockBackRadius; //Largura do espaço que a galera vai tomar knockbakc
    public float meleeRate; //Velocidade de ataque

    float nextMelee; //Quando efetivamente da pra atacar

    int shootableMask; //Mask shootable(layer)

    Animator myAnim; //Animator do player
    playerController myPC; //myPlayerController basicamente a variavel que vai guardar o playerController

    // Start is called before the first frame update
    void Start()
    {
        shootableMask = LayerMask.GetMask("Shootable"); //Posição do layer Shootable
        myAnim = transform.root.GetComponent<Animator>(); //Pega o Animator -- root se refere ao pai da hierarquia
        myPC = transform.root.GetComponent<playerController>(); //Que no caso é o Soldier
        nextMelee = 0f;        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float melee = Input.GetAxis("Fire2"); //Left Alt ou botão esquerdo mouse

        if(melee > 0 && nextMelee < Time.time && !(myPC.GetRunning())){
            myAnim.SetTrigger("gunMelee"); //Já é automaticamente true por um momento gatilho
            nextMelee = Time.time + meleeRate; //Tempo em segundos desde o inicio até "agora" + rate

            //do Damage
            //Cria array de colider que salva tudo que tiver dentro dessa overlap sphere
            //Parametros --- Lugar onde ela é criada, tamanho dela e masks que ela afetara
            Collider[] attacked = Physics.OverlapSphere(transform.position, knockBackRadius, shootableMask);

            for(int i = 0; i < attacked.Length; i++){
                if(attacked[i].GetComponent<Collider>().tag == "Enemy"){
                    enemyHealth theEnemyHealth = attacked[i].GetComponent<enemyHealth>(); //tenta achar o script de vida

                    if(theEnemyHealth != null){
                        theEnemyHealth.addDamage(damage);
                        theEnemyHealth.damageFX(transform.position, -transform.forward);
                    } 
                }
            }
        }
    }
}
