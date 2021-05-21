using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Permite usar recursos da UI do Unity

public class fireBullet : MonoBehaviour
{

    public float timeBetweenBullets = 0.15f; //Tempo entre os tiros
    public GameObject projectile; //O que ele atira, um objeto chamado projectile (nesse caso um prefab)

    float nextBullet; //Quando o proximo tiro pode ser atirado

    //Bullet info
    public int startRounds; //Numero de balas que vai começar
    public int maxRounds; //Maximo número de balas que uma arma pode carregar
    int remainingRounds; //O tanto de tiros que ainda tem, é com ele que o Slider é controlado

    public Slider playerAmmoSlider; //Link entre o script e o slider

    //Audio Info
    AudioSource gunMuzzleAS; //Variavel que vai guardar o AudioSource
    public AudioClip shootSound; //Variavel que guarda o som de tiro
    public AudioClip reloadSound; //Varivel que guarda o Clip de Audio do reload

    //Graphic info
    public Sprite weaponSprite; //O sprite que mostra no hud
    public Image weaponImage; //Parte do GUI que ta com a imagem, vou usar isso pra mudar

    // Start is called before the first frame update
    void Awake()
    {
        nextBullet = 0f; //Não tem espera, pode ser atirado imediatamente
        remainingRounds = startRounds; //Logo no começo, ajusta com o start e nao com o max
        playerAmmoSlider.maxValue = maxRounds; //Até que valor que o slider vai poder representar
        playerAmmoSlider.value = remainingRounds; //Começa indicando o valor de balas que ainda tem
        gunMuzzleAS = GetComponent<AudioSource>(); //Atribui o AudioSource na variavel
    }

    // Update is called once per frame
    void Update()
    {
        /*Intancia uma variavel do tipo player controler que vai pegar o script "playerController"*/
        /*Como ele esta no GunMuzzle ele vai até o root(Soldier) e pega o componente "playerController"*/
        playerController myPlayer = transform.root.GetComponent<playerController>();

        /*Input Raw(0 ou 1) do fire1 (botão esquerdo do mouse) for maior que 0(ou seja, esta apertado) e nextBullet for menor que Time.time*/
        /*E ainda tem tiros pra atirar*/
        if(Input.GetAxisRaw("Fire1")>0 && nextBullet<Time.time && remainingRounds > 0){
            //Reseta o tempo entre tiro
            nextBullet = Time.time + timeBetweenBullets;
            //Instancia um vetor pra rotação
            Vector3 rot;
            //Se a função GetFacing do script do player retornar -1 (ou seja, olhando pra esquerda) rotacionar o vetor
            //Necessario pq a função Flip não rotaciona mais inverte a escala em Z o que não é normal
            //Normalmente apenas o transform.forward do codigo do tiro ja seria suficiente
            if(myPlayer.GetFacing() == -1){
                rot = new Vector3(0,-90,0);
            }else{
                rot = new Vector3(0, 90, 0);
            }

            

            //Instancia o objeto efetivamente na cena, cria ele na cena
            //Quaternion - Sistemas de coordenadas matematicas que serve pra mexer com rotações e impede que algum axis de rotação "quebre"
            Instantiate(projectile, transform.position, Quaternion.Euler(rot));

            PlayASound(shootSound); //Chama a função que toca o som

            remainingRounds -= 1; //Voce atirou um, voce perde 1
            playerAmmoSlider.value = remainingRounds; //Muda o slider pra indicar pro slider que ele tem que abaixar
        }


        /* --- Reload Teste
        if(Input.GetAxisRaw("Reload")>0){
            remainingRounds = maxRounds; //Recarrega o valor
            playerAmmoSlider.value = remainingRounds; //Indica o reload pro slider
        }
        */
    }
    //Se quiser pode passar por parametro o tanto que quer recarregar
    public void reload(){
        remainingRounds = maxRounds; //Recarrega tudo
        playerAmmoSlider.value = remainingRounds; //Ja mexe no slider
        PlayASound(reloadSound); //Chama a função que toca som
    }


    //Recebe como parametro um AudioClip
    void PlayASound(AudioClip playTheSound){
        gunMuzzleAS.clip = playTheSound; //Atribui esse audioClip no AudioSource
        gunMuzzleAS.Play(); //Toca o som
    }

    public void initializeWeapon(){
        gunMuzzleAS.clip = reloadSound; //O clip agora é o de reload
        gunMuzzleAS.Play(); //Vai tocar o barulho de reload (estilo sla, um cszão)
        nextBullet = 0; //Zera o tempo pra atirar pra ja trocar e atirar foda se
        playerAmmoSlider.maxValue = maxRounds; //Atualiza o sider de munição pro valor máximo de munição da arma atual
        playerAmmoSlider.value = remainingRounds; //Atualiza o valor do slider pra ja mostrar a quantidade de munição
        weaponImage.sprite = weaponSprite; //Muda o sprite la em baixo pra indicar a mudança de armas
    }
}
