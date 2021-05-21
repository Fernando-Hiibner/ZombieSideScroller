using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //Movement properties
    public float runSpeed; //Velocidade de corrida
    public float walkSpeed; //Velocidade de Andar
    bool runing; //Esta correndo ou andando

    Rigidbody myRB; //My Rigidbody
    Animator myAnim; //My Animator

    bool facingRight; //Se estiver olhando pra direita True, se for esquerda False

    //Jump properties
    bool grounded = false; //Inicalmemte ele não esta no chão, esta levemente no ar
    Collider[] groundCollisions; //Array de coliders (Esferas que ficaram abaixo dos pés)
    float groundCheckRadius = 0.2f; //Raio dos colliders
    public LayerMask groundLayer; //Vai pegar o layer que foi setado no chão (Plataforma - ground)
    public Transform groundCheck; //Vai pegar o atributo transform do game object empty (Ground Check)
    public float jumpHeight; //Altura do pulo --- jumpHeight
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>(); //Instancia o componente Rigidbody
        myAnim = GetComponent<Animator>(); //Instancia o componente Animator
        facingRight = true; //Inicia olhando pra direita
    }

    // Update is called once per frame can run faster or slower as the frame rate fluctuates
    void Update()
    {
        
    }
    //FixedUpadte is called in a constant time its not afected by the framerate
    void FixedUpdate(){

        runing = false; //Imadiatamente que ocorre o Fixed Update ele seta runing como false

        //Se grounded for true ou seja, estiver no chão e o input axis jump(barra de espaço) for maior que um
        if(grounded && Input.GetAxis("Jump")>0){
            grounded = false;
            myAnim.SetBool("grounded", grounded); //Seta a var grounded do componente animator para false
            myRB.velocity = new Vector3(myRB.velocity.x,0,0); //Zera a velocidade de y
            myRB.AddForce(new Vector3(0,jumpHeight,0)); //Adciona uma força no personagem via vector3(x,y,z) onde jumpHeiht é a forca(200)
        }

        //GroundCollision --- Arrays de Colliders
        //Biblioteca Physics método OverlapSphere(Posição a desenhar a esfera, raio da esfera, layer do chão)
        //OverlapSphere(groundCheck.positon --- Objeto transform que ta no pé do personagem, groundCheckRadius --- float setado la em cima
        //groundLayer --- LayerMask "ground" que é pega la em cima)
        groundCollisions = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
        //Se o tamanho da lista de collisores de ground foi maior que zero(Quer dizer que colidiu com o chão, ou seja, ta no chão) seta
        //grounded pra true, caso contrario, não esta no chão ou seja, grounded é false;
        if (groundCollisions.Length>0){
            grounded = true;
        }else{
            grounded = false;
        }

        //Seta a variavel bool grounded dentro do componente animator
        myAnim.SetBool("grounded", grounded);

        myAnim.SetFloat("vspeed", myRB.velocity.y);

        /*Variavel do tipo float que vai pegar o Input do Axis horizontal(<-,->,a,d) -- Padrão do Unity -- Olhar em File/Project Setings/Input*/
        float move = Input.GetAxis("Horizontal");

        /*Seta uma varivale float dentro do component animator, nesse caso com o nome "speed" e com o valor Mathf.Abs(move)
        Que é basicamente o valor absoluto do float move setado acima pego do input*/
        myAnim.SetFloat("speed", Mathf.Abs(move));

        //Variavel float que vai pegar o Axis RAW(RAAAAAAAW) do input Fire3(left Shifit) ou seja, ao inves de pegar qualquer valor entre -1>0>1
        //ele pega ou 0 ou 1 --- Fire3 associado por padrão ao Left Shift no Unity
        float sneaking = Input.GetAxisRaw("Fire3");
        /* Seta a variavel float "sneaking" no objeto animator com o valor de sneaking pego ali em cima*/
        myAnim.SetFloat("sneaking", sneaking);

        //Caso o botão de tiro estiver apertado naturalmente estou atirando logo é "True"
        float firing = Input.GetAxis("Fire1");
        //Seta a variavel firing no shooting do animator
        myAnim.SetFloat("shooting", firing);

        /*Seta a velocidade do componente rigidbody, usando um vetor3(x,y,z) -- velocidade de X é float move que é pego pelo input
        do Axis vezes runSpeed, velocidade em y deve continuar como está então é pego o valor de velocidade de y do proprio rigidbody
        valocidade de z deve sempre ser 0 (Por ser um jogo 2.5D)*/
        /*Funciona dentro de um if que pega o valor de sneaking, que se for maior que 0 quer dizer "SIM" ou seja, esta agachado e então
        o calculo de velocidade de movimento é feito a partir do walkSpeed, e deve estar no chão, senão ele vai pular devagar
        se for menor que 0 quer dizer "NÃO" ou seja, não esta agachado então o calculo de velocidade é feito a partir do valor de runSpeed*/
        if ((sneaking>0 || firing>0) && grounded){
            myRB.velocity = new Vector3(move * walkSpeed, myRB.velocity.y, 0);
        }else{
            myRB.velocity = new Vector3(move * runSpeed, myRB.velocity.y, 0);
            if(Mathf.Abs(move) > 0) runing = true; //Se valor absoluto de matematica for maior que zero então running é zero (Evita setar true quando estiver idle)
        }

        //ifs responsaveis por checar velocidade de movimento move e direção que se esta olhando para executar o flip
         // se move maior que 0 (ou seja "D" ou ir pra direita) e facingRight = false (!facingRight) ele da Flip()
        if (move>0 && !facingRight) Flip();
        // se move menor que 0 (ou seja "A" ou ir pra esquerda) e facingRight = true (facingRight) ele da Flip()
        else if (move < 0 && facingRight) Flip();
    }

    //Criação da Função Flip --- Modo recomendado de fazer --- Rotacionar no eixo Z
    //Modo que sera feito --- Setar o Z da escala para !z --- Praticamente um flip de 2D
    void Flip(){
        facingRight = !facingRight; //facingRight vai inverter pq se voce for flipar naturalemte ele inverte
        Vector3 theScale = transform.localScale; //Cria um vector3(x,y,x) e atribui a esses valores os valores de Scale no menu Transform
        theScale.z *= -1; //Multiplica o z do vector3 theScale por -1 e inverte o valor
        transform.localScale = theScale; //Substitui os tres valores do componente scale de transform pelo novo vector3 theScale
    }

    public float GetFacing(){
        if(facingRight) return 1;
        else return -1;
    }

    public bool GetRunning(){
        return runing; //Função get set 2.0 Remaster kkkk
    }
}
