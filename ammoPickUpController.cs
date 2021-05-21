using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickUpController : MonoBehaviour
{

    //public int ammoAmount; //Numero de balas que vai recarregar

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
            other.GetComponentInChildren<fireBullet>().reload(); //Pega no Children pq o script ta no muzzle na arma
            //GetComponent // GetComponentInParent // GetComponentInChildren
            Destroy(transform.root.gameObject); //Destroi o pai desse objeto
        }
    }
}
