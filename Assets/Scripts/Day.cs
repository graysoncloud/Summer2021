using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        List<int> randomEasy = Enumerable.Range(0, easyContracts.Count).ToList();
        List<int> randomHard = Enumerable.Range(0, hardContracts.Count).ToList();

        int randomChoice;


        for(int i = 0; i < numEasy; i++)
        {
            randomChoice = Random.Range(0, randomEasy.Count);
            contracts.Add(easyContracts[randomEasy[randomChoice]]);
            randomEasy.RemoveAt(randomChoice);
        }

        for(int i = 0; i < numHard; i++)
        {
            randomChoice = Random.Range(0, randomHard.Count);
            contracts.Add(hardContracts[randomHard[randomChoice]]);
            randomHard.RemoveAt(randomChoice);
        }

    }

}
