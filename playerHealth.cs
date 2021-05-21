using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Da acesso aos elementos da UI do Unity
public class playerHealth : MonoBehaviour
{

    public float fullHealth; //Vida máxima do personagem
    float currentHealth; //Vida do atual personagem

    public GameObject playerDeathFX;

    //HUD
    public Slider playerHealthSlider; //Slider que indica a vida do player no HUD
    public Image playerDamageScreen; //Imagem de sangue que aparece quando o personagem toma dano
    Color flashColor = new Color(255f,255f,255f,1f); //RGBA (Red,Grenn,Blue,Alfa) --- Todos brancos não muda  a cor da imagem
    float flashSpeed = 5f; //Velocidade que ele vai mudar esse cor e depois mudar dnvo
    bool damaged = false; //Vai ser usada pra detectar o dano e setar a cor

    public restartGame theGameController; //Vai ser usado pra instanciar o código de restart game


    public Text endGameText; //Texto da tela de morte

    AudioSource playerAS; //Variavel que vai guardar um audio source




    // Assim que o jogo carrega Awake é chamado
    void Awake()
    {
        currentHealth = fullHealth; //Começa com a vida máxima
        playerHealthSlider.maxValue = fullHealth; //Seta o maxValue do slider para a vida máxima
        playerHealthSlider.value = currentHealth; //Seta o slider para o valor máximo dele que no inicio do jogo é a vida máxima
        playerAS = GetComponent<AudioSource>(); //Pega efetivamene o AudioSource 
    }

    // Update is called once per frame
    void Update()
    {
        if(damaged){
            playerDamageScreen.color = flashColor; //Vai mudar a cor da imagem pra cor de flash color(Fazendo ela aparecer)
        }else{
            playerDamageScreen.color = Color.Lerp(playerDamageScreen.color, Color.clear, flashSpeed*Time.deltaTime); //Lerp --- Passa de um pra outro em um tempo determinado
            //Paramentros do Lerp(Inicial, até fim (Color.clear == (0,0,0,0), e a velocidade que vai fazer o Lerp*tempo entre os frame anterior e esse))
        }
        damaged = false; //Roda só depois do IF
    }

    public void addDamage(float damage){
        currentHealth -= damage; //Toma dano (vida atual menos dano)
        playerHealthSlider.value = currentHealth; //Abaixa o slider de acordo com a vida
        damaged = true; //Quando tomar dano seta damaged pra true ai o update pega isso roda o if e depois poe false dnvo
        //If se ele estiver morto
        playerAS.Play(); //Não precisa especificar pq ele ja ta la
        if(currentHealth <= 0){
            makeDead();
        }
    }

    //Função que as coisas que dão vida vão acessar pra poder recuperar vida
    public void addHealth(float health){
        currentHealth += health; //Adciona essa vida na player
        //Caso isso recupere mais vida que o player pode ter, ele vai ficar com o máximo
        if(currentHealth > fullHealth){
            currentHealth = fullHealth;
        }
        playerHealthSlider.value = currentHealth; //Faz o slider representar a vida
    }

    public void makeDead(){
        //Instancia o sangue na posição do objeto e coloca o vetor com rotação de -90 no X igual no efeito
        Instantiate(playerDeathFX, transform.position, Quaternion.Euler(new Vector3(-90,0,0)));
        playerDamageScreen.color = flashColor; //A damageScreen vai aparecer e ficar quando morrer
        Destroy (gameObject);
        Animator endGameAnim = endGameText.GetComponent<Animator>(); //Pega o animator do engGameText
        endGameAnim.SetTrigger("endGame"); //Manda o trigger que toca animação de morte do player
        theGameController.restartTheGame();
    }
}
