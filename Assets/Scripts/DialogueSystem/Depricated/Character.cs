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
    BarneyWorkingNormal, BarneyWorkingStressed, BarneyListening, BarneyFrowning, BarneyQuizzical, BarneySmiling, BarneyAfraid, BarneyPanicAttack1, BarneyPanicAttack2, BarneyLeavingDesk,
    MariaNeutral, MariaSmirk, MariaAngry,
    WesleyNeutral, WesleySmirk, WesleyLaughing, WesleySinister,
    RobbieNeutral, RobbieRelaxed, RobbieSmile, RobbieParanoid,
    ElizabethNeutral, ElizabethLaughing, ElizabethCoughing, ElizabethCoughingBad,
    WesleyBigLaugh,
}
