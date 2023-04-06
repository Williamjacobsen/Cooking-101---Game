using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            TextBox = GameObject.FindGameObjectWithTag("TextBox");
            TextBoxText = GameObject.FindGameObjectWithTag("TextBoxText");
            Dim = GameObject.FindGameObjectWithTag("Dim");
            DimBtn = GameObject.FindGameObjectWithTag("DimBtn");
            TextBox.SetActive(tasksState);
            Dim.SetActive(tasksState);
            DimBtn.SetActive(tasksState);

            tasks = JsonHandler.ReadJson();
            handleGuideSteps();
            Guide();
        }
    }

    private void handleGuideSteps()
    {
        for (int i = 0; i < tasks[0].guide.Length; i++)
        {
            tmpList.Add(tasks[0].guide[i].ToString().Split(" "));
        }

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
    
    public void ToolsBtn()
    {
    }

    public void FoodsBtn()
    {
    }

    public void FridgeBtn()
    {
        SceneManager.LoadScene("Fridge");
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
