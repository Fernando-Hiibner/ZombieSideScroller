using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponPickUpController : MonoBehaviour
{

    public int wichWeapon; //Qual o "ID" da arma que pegamos, index dela na lista de weapons no inventory manager do personagem
    public AudioClip pickUpClip; //Audio de pickUp

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            other.gameObject.GetComponent<InventoryManager>().activateWeapon(wichWeapon); //Usa essa função do código de inventario do player
            Destroy(transform.root.gameObject); //Destroi o pai dele
            AudioSource.PlayClipAtPoint(pickUpClip, transform.position); //Toca esse audio la mesmo que ele seja destruido
        }
    }
}
