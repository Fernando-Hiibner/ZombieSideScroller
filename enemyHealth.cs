using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Acesso aos elemento da interface (UI)

public class enemyHealth : MonoBehaviour
{

    public float enemyMaxHealth; //Maximo de vida que ele pode ter
    public float damageModifier; //Multiplicador de dano (Que vai ser usado como "Defesa")
    public GameObject damageParticles; //Particulas de dano
    public bool drops; //Pode ou não dropar
    public GameObject[] droplist; //Dropa alguma coisa, pode ser uma lista com drops que são randomizados depois
    public AudioClip deathSound; //Som de morte
    public bool canBurn; //Pode ou não pegar fogo
    public float burnDamage; //Dano de fogo
    public GameObject burnEffects; //Efeitos de fogo(Particulas de fogo que ja esta no zumbi)
    public float burnTime; //Tempo que ele fica pegando fogo

    bool onFire; //Esta pegando fogo
    float nextBurn; //Quando ele vai pegar fogo
    float burnInterval = 1f; //Intervalo entre o fogo
    float endBurn;

    public float knockbackForce; //Força de knockback

    float currentHealth; //Vida atual

    public Slider enemyHealthIndicator; //Slider de vida
    
    AudioSource enemyAS;

    // Assim que o jogo carrega Awake é chamado
    void Awake()
    {
        currentHealth = enemyMaxHealth; //começa com vida máxima
        enemyHealthIndicator.maxValue = enemyMaxHealth; //Maximo de vida que o slider pode mostrar
        enemyHealthIndicator.value = currentHealth; //Vida atual que o slider vai mostrar
        enemyAS = GetComponent<AudioSource>(); //Pega o audioSource
    }

    // Update is called once per frame
    void Update()
    {
        if(onFire && Time.time > nextBurn){
            addDamage(burnDamage); //Da o dano de fogo
            nextBurn += burnInterval; //Adciona o intervalo de dano de fogo no tempo do next burn
        }
        if(onFire && Time.time > endBurn){
            onFire = false; //Diz que não ta mais pegando fogo
            burnEffects.SetActive(false); //Desativa os efeitos de fogo
        }
    }

    public void addDamage(float damage){
        enemyHealthIndicator.gameObject.SetActive(true); //Ativa o enemyHealthIndicator que começa desativado
        damage = damage * damageModifier; //Multiplica o dano pelo multiplicador de dano
        if(damage <= 0){
            return; //Se não der dano sai da função
        }
        currentHealth -= damage; //Tira vida obviamente
        enemyHealthIndicator.value = currentHealth; //Muda o valor do slider pra vida atual
        zombieController aZombie = GetComponentInChildren<zombieController>();
        if(aZombie != null){
            Debug.Log("É zumbi");
            Rigidbody zombieRB = GetComponent<Rigidbody>(); //Pega o rigidbody do zombie
            zombieRB.velocity = Vector3.zero;
            if(aZombie.GetFacing() == 1){
                Debug.Log("Direita");
                zombieRB.AddForce(new Vector3(-knockbackForce,0,0), ForceMode.Impulse);
            }else if(aZombie.GetFacing() == -1){
                Debug.Log("Esquerda");
                zombieRB.AddForce(new Vector3(knockbackForce,0,0), ForceMode.Impulse);
            }
        }
        enemyAS.Play(); //Toca o som de dano
        if(currentHealth <= 0){
            makeDead(); //Chama a função que cuida do que deve acontecer quando ele morre
        }
    }

    public void addFire(){
        if(!canBurn){
            return; //Sai da função aqui se o inimigo é imune a fogo
        } 
        onFire = true;
        burnEffects.SetActive(true); //Atiiva o efeito de fogo do zumbi
        endBurn = Time.time + burnTime; //Esquema de fazer os tempos serem covertidos pra segundos
        nextBurn = Time.time + burnInterval; //Intervalo em segundos entre um dano e o outro
    }
    
    //Vai receber dois vetores3 que contem a transform position e a transform rotation
    //Onde deveram ser intanciados os efeitos de dano
    public void damageFX(Vector3 point, Vector3 rotation){
        Instantiate(damageParticles, point, Quaternion.Euler(rotation)); //Intancia as particulas de dano
    }

    //Não é publico pq é acessado apenas pelos elementos da classe
    void makeDead(){
        //turnOffMovement
        zombieController aZombie = GetComponentInChildren<zombieController>(); //Pega o script zombie controller do filho zombieDetect --- se não tive então não é um zumbi

        if(aZombie != null){
            aZombie.ragdollDeath();
        }

        //Create Ragdoll --- Ai sim hein
        AudioSource.PlayClipAtPoint(deathSound, transform.position, 1f); //Toca esse deathSound aqui mesmo se o objeto for destruido
        Destroy(gameObject.transform.root.gameObject); //Pega o pai do objeto e destroi ele de uma vez, funciona assim pq esse script pode ser usado em outros inimigos
        if(drops) Instantiate(droplist[Random.Range(0, droplist.Length)], transform.position, Quaternion.identity); //Dropa alguma coisa, nessa posição com essa rotaçaõ(Não rotaciona no caso, mantem a rotação original do que instanciou)
    }
}
