using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Btn : MonoBehaviour
{
    private bool tasksState = false;
    private GameObject TextBox;
    private GameObject TextBoxText;
    private GameObject Dim;
    private GameObject DimBtn;
    private int tasksGuideState = 0;
    private List<GameData> tasks;
    private string text;
    private List<string[]> tmpList = new List<string[]>();
    private List<string> list = new List<string>();
    public Sprite[] EggsSprite;
    public Sprite[] toolsList;
    public Sprite[] foodsList;
    public GameObject FoodElement;
    public GameObject Element;
    private GameObject foodsArea;
    private GameObject toolsArea;
    private GameObject egg_anim;
    private Animator egg_anim_comp;
    private Transform egg_anim_trans;
    private GameObject Canvas;
    private TextMeshProUGUI Counter;
    private Transform ClockField;
    private Transform FinishedRecipe;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            Inventory.InitializeToolsInventory();
            
            TextBox = GameObject.FindGameObjectWithTag("TextBox");
            TextBoxText = GameObject.FindGameObjectWithTag("TextBoxText");
            Dim = GameObject.FindGameObjectWithTag("Dim");
            DimBtn = GameObject.FindGameObjectWithTag("DimBtn");
            TextBox.SetActive(tasksState);
            Dim.SetActive(tasksState);
            DimBtn.SetActive(tasksState);
            foodsArea = GameObject.FindGameObjectWithTag("FoodsArea");
            foodsArea.SetActive(false);

            tasks = JsonHandler.ReadJson();
            handleGuideSteps();
            Guide();
        } 
        else if (SceneManager.GetActiveScene().name == "Stove" && gameObject.name == "Btn")
        {
            Inventory.InitializeToolsInventory();

            foodsArea = GameObject.FindGameObjectWithTag("FoodsArea");
            foodsArea.SetActive(false);
            toolsArea = GameObject.FindGameObjectWithTag("ToolsArea");
            toolsArea.SetActive(false);

            Counter = GameObject.FindGameObjectWithTag("MinutesCounter").GetComponent<TextMeshProUGUI>();
            ClockField = GameObject.FindGameObjectWithTag("ClockField").GetComponent<Transform>();
            ClockField.position = new Vector3(9999, 15, 5); 
            // bug fix, i move it away rather than hiding it, because the i cannot access it

            FinishedRecipe = GameObject.FindGameObjectWithTag("FinishedRecipe").GetComponent<Transform>();
            FinishedRecipe.position = new Vector3(9999, 15, 5);
        }
        
        if (gameObject.name == "Æg" || SceneManager.GetActiveScene().name == "Stove")
        {
            egg_anim = GameObject.FindGameObjectWithTag("egg_anim");
            egg_anim_comp = egg_anim.GetComponent<Animator>();
            egg_anim_comp.enabled = false;
            egg_anim_trans = egg_anim.GetComponent<Transform>();
            egg_anim_trans.position = new Vector3(750, 350, -9999);
        }
    }

    private void handleEggAnim()
    {
        if (egg_anim_trans.position.z == -9999)
        {
            egg_anim_trans.position = new Vector3(697, 297, 0);
            egg_anim_comp.enabled = true;
            StartCoroutine(OnCompleteAnimation(egg_anim_comp));
        }
    }

    IEnumerator OnCompleteAnimation(Animator animObj)
    {
        while(animObj.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) // animation is playing
            yield return null;
            
        foodsArea = GameObject.FindGameObjectWithTag("FoodsArea");
        foodState = false;
        foodsArea.SetActive(foodState);
        egg_anim_trans.position = new Vector3(700, 300, -9999);
        GameObject Background = GameObject.FindGameObjectWithTag("Background");
        Background.name = "egg_cooking_not_finished";
        Background.GetComponent<Image>().sprite = Resources.Load<Sprite>("egg_cooking_not_finished");

        GameObject.FindGameObjectWithTag("ClockField").GetComponent<Transform>().position = new Vector3(538, 279, 5);
    }

    private int delay = 0;
    public void MinuteAdd()
    {
        delay++;
        Counter.text = "Minutter: " + delay;
    }
    public void MinuteRemove()
    {
        if (delay > 0)
        {
            delay--;
            Counter.text = "Minutter: " + delay;
        }
    }
    public void TimerStart()
    {
        StartCoroutine(ClockAnimation());
    }

    IEnumerator ClockAnimation()
    {
        Debug.Log("delay: " + delay);
        int clockIdx = 1;
        GameObject Clock = GameObject.FindGameObjectWithTag("Clock");
        Image Clock_tansform = Clock.GetComponent<Image>();
        for (int i = 0; i <= delay; i++)
        {
            Debug.Log("clockIdx: " + clockIdx);
            Counter.text = "Minutter: " + (delay - i);
            Clock_tansform.sprite = Resources.Load<Sprite>("clock_frames/" + clockIdx);
            clockIdx++;
            if (clockIdx > 4)
            {
                clockIdx = 1;
            }
            yield return new WaitForSeconds(1);
        }
        ShowFinshedRecipe();
    }

    private void ShowFinshedRecipe()
    {
        ClockField.position = new Vector3(9999, 15, 5);
        FinishedRecipe.position = new Vector3(540, 280, 0);
    }

    private void handleGuideSteps()
    {
        for (int i = 0; i < tasks[0].guide.Length; i++)
        {
            tmpList.Add(tasks[0].guide[i].ToString().Split(" "));
        }

        // splits string into a newline for every 8 words
        string tmp;
        for (int i = 0; i < tmpList.Count; i++)
        {
            tmp = "";
            for (int j = 0; j < tmpList[i].Length; j++)
            {
                tmp += tmpList[i][j] + " ";
                if (j % 8 == 0 && j != 0) {
                    tmp += "\n";
                }
            }
            list.Add(tmp);
        }
    }

    private void Guide()
    {
        text = "";

        text += tasks[0].name + "\n\nIngredienser:\n";
        for (int i = 0; i < tasks[0].ingredients.Length; i++)
        {
            text += $"{tasks[0].ingredients[i]}\n";
        }

        text += "\nGuide:\n";
        text += list[tasksGuideState] + "\n\n";

        TextBoxText.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void TasksBtn()
    {
        tasksState = !tasksState;
        TextBox.SetActive(tasksState);
        Dim.SetActive(tasksState);
        DimBtn.SetActive(tasksState);
    }

    public void NextStepBtn()
    {
        if (tasksGuideState < list.Count - 1)
        {
            tasksGuideState++;
            Guide();
        }
        
        if (tasksGuideState == list.Count - 1)
        {
            GameObject.FindGameObjectWithTag("NextStepTaskBtn").GetComponent<TextMeshProUGUI>().text = "Complete";
        }
    }

    public void PreviousStepBtn()
    {
        
        if (tasksGuideState > 0)
        {
            tasksGuideState--;
            Guide();
            GameObject.FindGameObjectWithTag("NextStepTaskBtn").GetComponent<TextMeshProUGUI>().text = "Next Step";
        }
    }
    
    private void DestroyChildren(GameObject parent)
    {
        int kiddos = parent.transform.childCount;
        for (int i = 0; i < kiddos; i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
        return;
    }

    private bool toolState = false;
    public void ToolsBtn()
    {   
        toolState = !toolState;
        toolsArea.SetActive(toolState);
        foodState = false;
        foodsArea.SetActive(foodState);

        DestroyChildren(toolsArea);
        DestroyChildren(foodsArea);
        
        for (int i = 0; i < Inventory.tools.Count; i++)
        {
            if (Inventory.tools[i] == "Stegepande")
            {
                Transform ToolsArea = toolsArea.GetComponent<Transform>();
                GameObject El = Instantiate(Element);
                El.transform.SetParent(ToolsArea);
                GameObject icon = El.transform.GetChild(0).gameObject;
                icon.GetComponent<Image>().sprite = toolsList[0];
                El.transform.localScale = new Vector3(1, 1, 1);
                El.name = "Stegepande";
            }
        }
    }

    private bool foodState = false;
    public void FoodsBtn()
    {
        foodState = !foodState;
        foodsArea.SetActive(foodState);
        toolState = false;
        toolsArea.SetActive(toolState);

        DestroyChildren(toolsArea);
        DestroyChildren(foodsArea);
        
        for (int i = 0; i < Inventory.foods.Count; i++)
        {
            if (Inventory.foods[i] == "Æg")
            {
                Transform FoodsArea = foodsArea.GetComponent<Transform>();
                GameObject El = Instantiate(Element);
                El.transform.SetParent(FoodsArea);
                GameObject icon = El.transform.GetChild(0).gameObject;
                icon.GetComponent<Image>().sprite = foodsList[0];
                El.transform.localScale = new Vector3(1, 1, 1);
                El.name = "Æg";
            }
        }
    }

    public void HandlePan(string food)
    {
        GameObject Background = GameObject.FindGameObjectWithTag("Background");
        if (food == "")
        {
            if (Background.name == "komfurudenpande")
            {
                Background.name = "komfurmedpande";
                Background.GetComponent<Image>().sprite = Resources.Load<Sprite>("komfurmedpande");
            }
            else
            {
                Background.name = "komfurudenpande";
                Background.GetComponent<Image>().sprite = Resources.Load<Sprite>("komfurudenpande");
            }
        }
        else if (food == "Æg")
        {
            if (Background.name == "komfurmedpande")
            {
                handleEggAnim();
            }
        }
    }

    public void InventoryChildClicked()
    {
        switch (gameObject.name)
        {
            case "Stegepande":
                HandlePan("");
                break;
            case "Æg":
                HandlePan("Æg");
                break;
        }
    }

    public void FridgeBtn()
    {
        SceneManager.LoadScene("Fridge");
    }

    public void StoveBtn()
    {
        SceneManager.LoadScene("Stove");
    }

    private int EggsSpriteState = 0;
    private bool EggsTaken = false;
    public void EggsBtn()
    {
        if (EggsSpriteState == 0)
        {
            if (EggsTaken)
            {
                EggsSpriteState += 2;
            }
            else
            {
                EggsSpriteState++;
            }
        }
        else if (EggsSpriteState == 1 && !EggsTaken)
        {
            EggsSpriteState++;
            EggsTaken = true;
            Inventory.foods.Add("Æg");
            Debug.Log("Egg added to foods inventory.");
        }
        else if (EggsSpriteState == 2)
        {
            EggsSpriteState -= 2;
        }
        GameObject.FindGameObjectWithTag("Eggs").GetComponent<Image>().sprite = EggsSprite[EggsSpriteState];
    }

    public void BackBtn()
    {
        SceneManager.LoadScene("Game");
    }
}
