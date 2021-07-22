using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "Day", order = 1)]
public class Day : ScriptableObject
{
    public Sequence[] sequences;
    public List<Contract> contracts = new List<Contract>();

    public List<Contract> easyContracts = new List<Contract>();
    public List<Contract> hardContracts = new List<Contract>();
    public int numEasy;
    public int numHard;

    [System.Serializable]
    public class Sequence
    {
        // Could be a scene change, conversation, etc.
        public ScriptableObject initialEvent;
        public Trigger trigger;

        public enum Trigger
        {
            leavingHome,
            arrivingAtWork,
            solvedContract1,
            solvedContract2,
            solvedContract3,
            solvedContract4,
            solvedContract5,
            solvedContract6,
            leavingWork,
            dream,
            // Since going to bed and waking up is one combined sequence, waking up events must be specified the day before
            wakingUpNextDay
        }

    }

    public void ShuffleContracts()
    {

        // RESET
        contracts.Clear();
        List<int> randomEasy = new List<int>();
        List<int> randomHard = new List<int>();

        /*
        for(int i = 0; i < numEasy; i++)
        {
            int newNum = Random.Range(0, easyContracts.Count - 1);
            while(!randomEasy.Contains(newNum))
            {
                newNum = Random.Range(0, easyContracts.Count - 1);
            }
            randomEasy.Add(newNum);
            contracts.Add(easyContracts[newNum]);
        }

        for(int i = 0; i < numHard; i++)
        {
            int newNum = Random.Range(0, hardContracts.Count - 1);
            while(!randomHard.Contains(newNum))
            {
                newNum = Random.Range(0, hardContracts.Count - 1);
            }
            randomHard.Add(newNum);
            contracts.Add(hardContracts[newNum]);
        }

    }*/
    }

}
