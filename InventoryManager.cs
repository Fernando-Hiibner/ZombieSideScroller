using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] weapons; //Array de armas
    bool[] weaponsAvailable; //Quais armas estão disponiveis pra uso
    public Image weaponImage; //Imagem da arma
    
    int currentWeapon; //Arma atual

    Animator weaponImageAnim; //Animator da weaponImage


    // Start is called before the first frame update
    void Start()
    {  
        weaponsAvailable = new bool[weapons.Length]; //Dessa forma o weaponsAvailable sera uma lista de bool do mesmo tamanho que a lista de armas
        for(int i = 0; i < weapons.Length; i++){
            weaponsAvailable[i] = false; //Inicialmente não temos nem uma arma disponivel
        } 
        currentWeapon = 0; //LMG, ou a arma que estiver na nossa mão é setada como a arma zero do array
        weaponsAvailable[currentWeapon] = true; //Agora temos disponivel a LMG(Exemplo, poderia ser outra arma) 
        /*
        for(int i = 0; i < weapons.Length; i++){
            weaponsAvailable[i] = true; //Motivos de teste!!!!
        }
        */

        weaponImageAnim = weaponImage.GetComponent<Animator>(); //Pega o animator da weaponImage

        deactivateWeapons(); //De inicio desativa todas as armas

        setWeaponActive(currentWeapon); //Ativa nossa arma atual(LMG)

    }

    // Update is called once per frame
    void Update()
    {
        //Togle Weapon
        if(Input.GetButtonDown("Submit")){
            int i; //Declara um i
            for(i =currentWeapon+1; i < weapons.Length; i++){
                if(weaponsAvailable[i] == true){
                    currentWeapon = i; //Arma atual é igual a i
                    setWeaponActive(currentWeapon); //Ativa a nova arma
                    return;
                }
            }
            for(i = 0; i < currentWeapon; i++){
                if(weaponsAvailable[i] == true){
                    currentWeapon = i; //Arma atual é igual a i
                    setWeaponActive(currentWeapon); //Ativa a nova arma
                    return;
                }               
            }
        }
    }

    public void setWeaponActive(int wichWeapon){
        //Se essa arma do int na lista de availables for false, essa arma esta indisponivel então vaza
        if(!weaponsAvailable[wichWeapon]) return; //Retorna e sai da função
        deactivateWeapons(); //desativa todas
        weapons[wichWeapon].SetActive(true); //Seta a arma que queremos como ativa
        weapons[wichWeapon].GetComponentInChildren<fireBullet>().initializeWeapon(); //Vai procurar o script fire bullet nos filhos da arma pai e "inicia a arma"
        weaponImageAnim.SetTrigger("weaponSwitch"); //Manda o trigger pro anim pra fazer a animação
    }

    void deactivateWeapons(){
        for(int i = 0; i < weapons.Length; i++) weapons[i].SetActive(false); //Desativa todos as armas de inicio
    }
    public void activateWeapon(int wichWeapon){
        weaponsAvailable[wichWeapon] = true; //Ativa a arma do parametro
    }
}
