using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    public AnimationName defaultAnimation;
    public Vector3 startLocation;

    private void Start()
    {

    }

}

public enum CharacterName { 
    None,
    Barney, 
    Elizabeth, 
    Wesley, 
    Robbie, 
    Maria,
    Elliana,
    RichRamirez
}

public enum AnimationName
{
    BarneyWorkIdle, BarneyListening, BarneySmile, BarneyQuizzical, BarneyFear,
    MariaWalking, MariaIdle, MariaSmirk, MariaAngry,
    WesleyWalking, WesleyIdle, WesleyLaughing, WesleySinister,
    RobbieWalking, RobbieIdle, RobbieSmile, RobbieParanoid,
    ElizabethIdle, ElizabethLaughing, ElizabethCoughing, ElizabethCoughingBad,
}
