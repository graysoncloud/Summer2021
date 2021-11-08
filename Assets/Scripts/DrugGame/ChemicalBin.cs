using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalBin : MonoBehaviour
{

    // In the full development, there would be a "ChemicalBin" abstract class that this would inherit from
    public Chemical ChemicalPrefab;
    public TMPro.TextMeshProUGUI title, costDisplay;
    public Transform graphicPosition;
    public GameObject InfoTooltip;
    [SerializeField] private Chemical chemGraphic = null;
    [SerializeField] private Color undesiredColor, desiredColor, optionalDesiredColor, optionalUndesiredColor;
    [SerializeField] private SpriteRenderer border;

    private void Start()
    {
        if (ChemicalPrefab != null)
        {
            if (title != null)
            {
                title.SetText(ChemicalPrefab.name);
            }
            if (costDisplay != null)
            {
                costDisplay.SetText("Cost " + ChemicalPrefab.getCost().ToString());
            }
        }
    }

    public void SetGlow()
    {
        Contract currentContract = GameManager.instance.GetCurrentContract();
        //Debug.Log(currentContract.companyName);
        bool desired = false, undesired = false, optionalDesired = false, optionalUndesired = false, glow = false;
        foreach (var effect in ChemicalPrefab.effects)
        {
            if (currentContract.usesDesirableEffect && effect == currentContract.desirableEffect)
            {
                desired = true;
                glow = true;
            }
            if (currentContract.usesUndesirableEffect && effect == currentContract.undesirableEffect)
            {
                undesired = true;
                glow = true;
            }
            if (currentContract.usesOptionalDesireable && effect == currentContract.optionalEffect)
            {
                optionalDesired = true;
                glow = true;
            }
            if (currentContract.usesOptionalUndesirable && effect == currentContract.optionalEffect)
            {
                optionalUndesired = true;
                glow = true;
            }
        }
        if (!glow)
            border.color = Color.clear;
        if (optionalDesired)
            border.color = optionalDesiredColor;
        else if (optionalUndesired)
            border.color = optionalUndesiredColor;
        else if (undesired)
            border.color = undesiredColor;
        else if (desired)
            border.color = desiredColor;
    }

    private void OnMouseOver(){

        if(DrugManager.instance.currentlyHeldChemical != null)
        {
            InfoTooltip.SetActive(false);
        }

        if(!InfoTooltip.activeSelf && !GameManager.instance.optionsMenuActive && DrugManager.instance.currentlyHeldChemical == null)
        {
            InfoTooltip.SetActive(true);
            InfoTooltip.GetComponent<ChemicalInfoDisplayer>().ChangeText(ChemicalPrefab.getInfo());
        }
    }

    private void OnMouseExit(){
        if(InfoTooltip.activeSelf)
        {
            InfoTooltip.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (!Printer.instance.solutionPrinted && ActiveContractArea.instance.currentContract != null && !GameManager.instance.optionsMenuActive)
            CreateDrug();
    }

    public void CreateDrug()
    {
        if (DrugManager.instance.currentlyHeldChemical != null) return;

        if (ChemicalPrefab != null && !GameManager.instance.optionsMenuActive)
        {
            Chemical newChemical = Instantiate<Chemical>(ChemicalPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            newChemical.name = ChemicalPrefab.name;//without this, it adds (clone) to the name which is annoying if you try to use them as IDs
            DrugManager.instance.currentlyHeldChemical = newChemical;
            newChemical.isPlaced = false;
            newChemical.GetComponent<PolygonCollider2D>().enabled = false;
            SFXPlayer.instance.PlaySoundEffect(2);
            newChemical.setActive(false);

            foreach (SpriteRenderer SR in newChemical.GetComponentsInChildren<SpriteRenderer>())
            {
                SR.sortingLayerName = "LiftedTile";
            }
        }
    }

    public void CreateDrugGraphic()
    {
        chemGraphic = Instantiate<Chemical>(ChemicalPrefab, graphicPosition.position, Quaternion.identity);
        chemGraphic.setActive(false);
        chemGraphic.tag = "Untagged";
        chemGraphic.GetComponent<PolygonCollider2D>().enabled = false;
        chemGraphic.transform.localScale = new Vector3(3, 3, 1);
        chemGraphic.GetComponent<Chemical>().enabled = false;
    }

    public void RefreshChemGraphic()
    {
        if (chemGraphic != null)
        {
            Destroy(chemGraphic.gameObject);
        }
        CreateDrugGraphic();
    }

    public void ClearChemGraphic()
    {
        if (chemGraphic != null)
        {
            Destroy(chemGraphic.gameObject);
        }
    }

    public void ChangeCost(float cost)
    {
        costDisplay.SetText("Cost " + cost.ToString());
    }
    
}
