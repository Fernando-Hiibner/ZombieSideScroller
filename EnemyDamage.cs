using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    //Criação do Script comum a tudo que é "inimigo" seja objeto que da dano ou criatura

    public float damage; //Dano
    public float damageRate; //De quanto em quanto tempo se pode dar dano (pra não estuprar o cara assim que ele tocar o tank trap)
    public float pushBackForce; //Knockback 2.0 Renamed

    float nextDamage; //"Actual time when the next damage can occur"

    bool playerInRange = false; //Mostra quando o personagem esta no collider em SI --- So vai dar dano se for sim

    GameObject thePlayer; //Variavel do player em si o OBJETO PLAYER(SOLDIER)
    playerHealth thePlayerHealth; //Varivale que vai pegar a vida do player de dentro do script de vida do objeto acima

    // Start is called before the first frame update
    void Start()
    {
        nextDamage = Time.time;
        thePlayer = GameObject.FindGameObjectWithTag("Player"); //Encontra o objecto player pela sua TAG de Player
        thePlayerHealth = thePlayer.GetComponent<playerHealth>(); //Pega o script playerHealth associado ao objeto PLayer(SOLDIER)
    }

    // Update is called once per frame
    void Update()
    {
        //Checa a cada frame se o player esta no alcance
        if(playerInRange){
            Attack();
        }
    }
    //Esse metodo reconhece a entrada de algo no 
    //collider e detecta o objeto do outro colider e atribui a variavel do parametro nesse caso other
    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            //Se a tag do que entrou no collider for "Player" seta a var playerInRange como true
            playerInRange = true;
        }
    }

    //Faz o mesmo que o de cima mas detecta a saida
    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            playerInRange = false;
        }
    }
    //Void que é responsavel por atacar
    void Attack(){
        //Se o tempo de proximo ataque for menor que Time.time (Tempo em segundos contado a cada começo de frame que diz a quantos segundos o jogo iniciou)
        if(nextDamage<=Time.time){
            thePlayerHealth.addDamage(damage); //Vai chamar a função addDamage do script e mandar o damage
            nextDamage = Time.time + damageRate; //Soma o Time.time aqui senão ia ser impossivel alcancar ele e o damage rate que se for bem alto soma mais e pode atacar mais rapido
        
            pushBack(thePlayer.transform); //Função que vai dar knockback no player usando o transform
        }
    }

    //Função que vai dar knockback no player usando o transform
    void pushBack(Transform pushedObject){
        //Cria uma variavel Vetor3 que vai definir a velocidade de pushBack
        //Vai usar como base a posição que o player esta e subtrair da posição do emisor de dano
        //normalized vai dar um numero so que representa o vetor (sla como crls isso é possivel MATH BOYS)
        Vector3 pushDirection = new Vector3(0,(pushedObject.position.y - transform.position.y),0).normalized;
        pushDirection *= pushBackForce; //multiplica o valor normalizado do vetor acima pela forca de pushback
        //Pega o rigidbody do objeto que vai tomar knockback
        Rigidbody pushedRB = pushedObject.GetComponent<Rigidbody>();
        //Anula qualquer velocidade que aquele objeto tem
        pushedRB.velocity = Vector3.zero;
        //Adciona força naquele objeto no modo Impulse
        pushedRB.AddForce(pushDirection,ForceMode.Impulse);
    }

}
