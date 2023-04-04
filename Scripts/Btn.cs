using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Btn : MonoBehaviour
{
    private bool tasksState = false;
    private GameObject TextBox;
    private GameObject TextBoxText;
    private GameObject Dim;
    private GameObject DimBtn;
    int tasksGuideState = 0;
    private List<GameData> tasks;
    private string text;
    private List<string[]> tmpList = new List<string[]>();
    private List<string> list = new List<string>();

    private void Start()
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
}
