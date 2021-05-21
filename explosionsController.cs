using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionsController : MonoBehaviour
{

    public Light explosionLight;
    public float power; //Poder da explosão
    public float radius; //Raio de dano
    public float damage; //Dano que a explosão dá
    public AudioClip explosionAudio; //Som da explosao

    // Start is called before the first frame update
    void Awake()
    {
        Vector3 explosionPos = transform.position; //Pega a posição do objeto pra criar a Overlap Sphere
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius); //Cria a overlap sphere naquele ponto com aquele raio
        foreach (Collider hit in colliders){
            Rigidbody rb = hit.GetComponent<Rigidbody>(); //Pega o rigidbody do que foi acertado
            //Se rb foi encontrado no objeto
            if(rb != null){
                Debug.Log("Acertou");
                //Built-in function --- Forca da explosao, ponto da onde explodiu, raio
                //Modificador que joga pra cima, modo de forca = impulso
                rb.AddExplosionForce(power, explosionPos, radius, 3.0f, ForceMode.Impulse);
                if(hit.tag == "Player"){
                    playerHealth thePlayerHealth = hit.gameObject.GetComponent<playerHealth>(); //Pq aqui é so o Collider
                    thePlayerHealth.addDamage(damage);
                }else if(hit.tag == "Enemy"){
                    enemyHealth theEnemyHealth = hit.gameObject.GetComponent<enemyHealth>();
                    theEnemyHealth.addDamage(damage);
                    theEnemyHealth.addFire();
                }
            }
        }
        AudioSource.PlayClipAtPoint(explosionAudio, transform.position, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        //LERP --- Suaviza a transição entre dois valores
        explosionLight.intensity = Mathf.Lerp(explosionLight.intensity, 0f, 5*Time.time);
        //Vai da intensidade original, até 0, nesse tempo
    }
}
