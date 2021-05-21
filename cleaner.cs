using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cleaner : MonoBehaviour
{



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
            //Vai pegar o objeto dono do colider "other" e desse objecto pegar o componente "playerHealth"
            playerHealth playerDead = other.gameObject.GetComponent<playerHealth>();
            playerDead.makeDead();
        }else{
            //Destroe o que cair ali e não for player
            //Root basicamente vai subir a hiearquia até chegar no pai de other e vai destruir esse gameObject
            Destroy(other.transform.root.gameObject);
        }
    }
}
