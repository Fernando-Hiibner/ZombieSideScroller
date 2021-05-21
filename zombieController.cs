using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieController : MonoBehaviour
{
    public GameObject flipModel; //Parte do zumbi que vai girar (Apenas o modelo, isso faz com que a Ui dele não gire por exemplo)
    
    //AudioOption
    public AudioClip[] idleSounds; //Lista de soms que o zumbi pode fazer enquanto em modo idle
    public float idleSoundTime; //De quanto em quanto tempo o zumbi pode fazer um som de idle
    AudioSource enemyMovementAS; //Audio de movimento
    float nextIdleSound = 0f; //Isso é so a variavel que recebe o valor de time e idleSoundTime pra ser usada nos ifs e efetivamente fazer demoraro tempo que defeiniu no idlesoundtime (parecido com ataque e tals)

    public GameObject ragdollZombie; //O zumbi Ragdoll

    public float detectionTime; //Tempo que o player tem que ficar na area de deteccão antes do zumbi ir atras
    //Melhor dizendo, tempo antes do zumbi comecar a correr quando estiver dentro da área de detecção, qualquer coisa abaixo que contraria isso é coisa de BRONY(Fernando do futuro pós video)
    float startRun; //nextIdleSound 2.0 Remaster
    bool firstDetection; //Marca a primeira detecção, uma vez detectado ja era irmão

    //Movement options
    public float walkSpeed; //Velocidade de caminhadas
    public float runSpeed; //Velocidade de corrida
    public bool facingRight = true; //Ja vai estar olhando pra direita mas la em baixo é random (SIM KKKKK)

    float moveSpeed; //Velocidade de movimento (Ou walk ou run Speed, muda de acordo com o que o zumbi ta fazendo, é ela que vai ser usada nos "Calculos")
    bool Runing; //Ta correndo? Verdade ou Mentira

    Rigidbody myRB; //Link com o rigidbody
    Animator myAnim; //Link com o Animator
    Transform detectedPlayer; //Em que lugar o player esta em relação ao zumbi, usado para definir em que direção o zumbi tem que estar olhando


    bool Detected; //Vai ficar true até alguem estar morto... É você ou ele -_-
    


    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponentInParent<Rigidbody>(); //esse e o de baixo pegam os seus respectivos componentes dentro
        myAnim = GetComponentInParent<Animator>(); //do pai, pq esses dois são elementos do top level

        enemyMovementAS = GetComponent<AudioSource>(); //Componente nele mesmo

        Runing = false; //Pq ta parado
        Detected = false; //Pq não detectou nada ainda, acabaou de nascer
        firstDetection = false; //Plru
        moveSpeed = walkSpeed; //moveSpeed começa como walk speed pq é o primeiro estagio de movimento dele

        //Basicamente 50% de chance de mudar o flip dele
        if(Random.Range(0, 10) > 5){
            Flip(); //Chama a função Flip
        }
    }

    // Pq o player anda no FixedUpdate e pq esse é um objeto com fisica
    void FixedUpdate()
    {   
        //Se o player for detectado
        if(Detected){
            
            //Se a posição x do player é menor que a nossa e estamos olhando pra direita então flip();
            //detectedPlayer equivale a other.transform
            if(detectedPlayer.position.x < transform.position.x && facingRight){
                Flip();
            }else if(detectedPlayer.position.x > transform.position.x && !facingRight){
                //Se a posição x d0 player é maior que a nossa e não estamos olhando pra direita então Flip();
                Flip();
            }//Mesma coisa dali de baixo só que aqui muda de uma vez pra caso o player mude de lado
            
            //Se não for a primeira detectção ver se ja é a hora de começar a correr, como isso é setado pra falso
            //Quando o player sai do alcance de detecção, basicamente vai ser usado pro zumbi começar a correr
            //Quando você estiver longe dele e ele puder correr(Tem controles pra isso que devem ser checados)
            if(!firstDetection){
                startRun = Time.time + detectionTime; //Primeiramente seta o tempo pra ele efetivamente começar a correr em segundos
                firstDetection = true; //A primeira detecção ocorreu ou seja, ele não entra dnvo nesse código e não fica entrando aqui e aumentando o tempo pra começar a correr
            }
        }
        if(Detected && !facingRight){
            //Vai mover ele pra esquerda normalmente não teria esse problema mas pela forma que o flip
            //funciona ele tem que ser feito dessa forma (Normalmente é só aplicar velocidade no x)
            myRB.velocity = new Vector3((moveSpeed*-1), myRB.velocity.y,0); //Põe pra correr em x negativo pq tem que ir pra esquerda afinal não esta olhando pra direita, y continua o mesmo, e z nunca muda, jogo 2D amigão
        }else if(Detected && facingRight){
            myRB.velocity = new Vector3(moveSpeed, myRB.velocity.y, 0); //Põe pra correr em positivo pq agora ele vai pra direita ja que esta olhando pra direita, y continua o mesmo, e z nunca muda, jogo 2D amigão
        }

        //Se não estamos correndo e ja detectamos algo
        if(!Runing && Detected){
            //Se ja passou a hora de correr
            if(startRun < Time.time){
                moveSpeed = runSpeed; //Troca a velocidade de movimento pra de corrida
                myAnim.SetTrigger("run"); //Manda o trigger pra sair do zombieWalk pro zombieRun
                Runing = true; //Pq agora ta correndo
            }   
        }

        //Idle or walking sounds
        //Se não esta correndo ou ta idle ou ta andando
        if(!Runing){
            //Se sair um número maior que 5 (Ou seja 50% de chance) e ja deu a hora de fazer o próximo som de Idle
            if(Random.Range(0,10)>5 && nextIdleSound < Time.time){
                //Nova var que vai pegar um Clip temporario aleatorio da lista de soms
                //Random.Range(0, até tamanho máximo da lista)
                AudioClip tempClip = idleSounds[Random.Range(0, idleSounds.Length)]; //Não confunda, colchetes pra indicar index, parenteses da função Random.Rande();
                enemyMovementAS.clip = tempClip; //Coloca esse clip no AudioSource do inimigo, clip é uma propriedade não um método
                enemyMovementAS.Play(); //Toca esse som
                nextIdleSound = idleSoundTime + Time.time; //Seta outra hora pra poder tocar o som de idle
            }
        }   
    }

    //Quando o collider de detecção Trigger encontra alguma coisa
    void OnTriggerEnter(Collider other){
        //Se esse Collider é de player e se ele não foi detectado ainda
        if(other.tag == "Player" && !Detected){
            Detected = true; //Foi detectado
            detectedPlayer = other.transform; //Atribui o transform do player que ele acabou de detectar a var do tipo "Transform" detectedPlayer
            myAnim.SetBool("detected", Detected); //Manda o true pro animator
            //Se a posição x do player é menor que a nossa e estamos olhando pra direita então flip();
            //detectedPlayer equivale a other.transform
            if(detectedPlayer.position.x < transform.position.x && facingRight){
                Flip();
            }else if(detectedPlayer.position.x > transform.position.x && !facingRight){
                //Se a posição x d0 player é maior que a nossa e não estamos olhando pra direita então Flip();
                Flip();
            }
        }

    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            firstDetection = false; //Se o player saiu da zona de colisão seta firstDetection pra false
            //Se estava correndo
            if(Runing){
                //Manda o trigger "Run" que em teoria vai sair do zombieRun pro zombieWalk (Olhar o Animator Poh)
                myAnim.SetTrigger("run");
                moveSpeed = walkSpeed; //Pq agora ele esta andando
                Runing = false; //Pq obviamente ele não ta mais correndo
            }
        }
    }

    //Ele muda uma vez no começo e depois só muda quando o player ta na área
    void Flip(){
        facingRight = !facingRight; //Basicamente inverte facingRight
        //Efetivamente flipa o personagem
        Vector3 theScale = flipModel.transform.localScale; //Coloca a local scale do modelo do zumbi num vetor 3 (local scale pq esse modelo é um children e queremos a escala dele)
        theScale.z *= -1; //Multiplicamos o valor Z dessa escala por -1, assim invertendo-0
        flipModel.transform.localScale = theScale; //Atribuimos esse vetor com o valor invertido como o localScale do modelo
    }

    public void ragdollDeath(){
        //Usa o transform no root pq esse script ta em um children de outro obj
        GameObject ragDoll = Instantiate(ragdollZombie, transform.root.position, Quaternion.identity) as GameObject; //as GameObject pq senão tenta instanciar como Object

        Transform ragDollMaster = ragDoll.transform.Find("master"); //Vai procurar por algo chamado master
        Transform zombieMaster = transform.root.Find("master");


        bool wasFacingRight = true; //Vai ajudar a descobri se foi flipado
        //Se não estamos olhando pra direita, ou seja estamos olhando pra esquerda
        if(!facingRight){
            wasFacingRight = false; //Agora fomos pra direita e antes não esvamos olhando pra direita e sim pra esquerda
            Flip(); //Flipamos
        }

        //Vai encontrar o transform de todas as juntas no ragdoll e todas as juntas no zombie em si(o que está prestes a morrer)
        Transform[] ragdollJoints = ragDollMaster.GetComponentsInChildren<Transform>(); //GetComponents no plurar vai pegar todos que achar
        Transform[] currentJoints = zombieMaster.GetComponentsInChildren<Transform>();

        //for que vai passar pelas duas listas procurando pelos nomes equivalentes das juntas do ragdoll e do zumbi
        for(int i = 0; i< ragdollJoints.Length; i++){
            for (int q = 0; q < currentJoints.Length; q++){
                //CompareTo função padrao, nesse caso ele vai comparar os nomes das posições "q" e "i" das suas listas e se não encontrar
                //diferença retorna 0
                //Lembrando que estamos comparando os nomes das juntas
                if(currentJoints[q].name.CompareTo(ragdollJoints[i].name) == 0){
                    ragdollJoints[i].position = currentJoints[q].position; //Posição do transform do zumbi ragdoll igual a do zumbi que ta morrendo
                    ragdollJoints[i].rotation = currentJoints[q].rotation; //Rotação do ragdoll igual a do zumbi
                    break; //Quebra a função pq não tem pq continuar procurando esse nome se ja achou
                }
            }
        }

        //Se ele estava olhando pra direita
        if(wasFacingRight){
            Vector3 rotVector = new Vector3(0,0,0);
            ragDoll.transform.rotation = Quaternion.Euler(rotVector); //Nesse caso a rotação vai se manter a mesma
        } else {
            Vector3 rotVector = new Vector3(0,90,0);
            ragDoll.transform.rotation = Quaternion.Euler(rotVector);
        }

        //Garantir se o zumbi ragdoll tera a mesma skin do zumbi original
        Transform zombieMesh = transform.root.transform.Find("zombieSoldier");
        Transform ragdollMesh = ragDoll.transform.Find("zombieSoldier");

        ragdollMesh.GetComponent<Renderer>().material = zombieMesh.GetComponent<Renderer>().material; //Coloca o material do zombie no ragdoll
    }

    public float GetFacing(){
        if (facingRight) return 1;
        else return -1;
    }
}
