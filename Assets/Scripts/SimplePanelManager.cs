using UnityEngine;
using System.Collections.Generic;

public class SimplePanelManager : MonoBehaviour
{
    public string DefaultPanelID;
    public bool showDefaultPanelOnStart = true;

    Dictionary<string,SimplePanel> panels = new Dictionary<string, SimplePanel>();
    Stack<SimplePanel> panelStack = new Stack<SimplePanel>();

    SimplePanel currentPanel;
    public void ShowDefaultPanel()
    {
        ShowPanel(DefaultPanelID);
    }
    private void Start()
    {
        if (!string.IsNullOrEmpty(DefaultPanelID) && showDefaultPanelOnStart)
            ShowPanel(DefaultPanelID);
    }

    public void AddPanel(SimplePanel panel)
    {
        if(string.IsNullOrEmpty(panel.panelID))
        {
            Debug.LogError($"Panel GameObject {panel.name}: tried to pass an empty Panel ID");
            return;
        }
        else if(panels.ContainsKey(panel.panelID))
        {
            Debug.LogError($"Duplicate key with {panel.panelID} found in Panel UI Manager");
            return;
        }        

        panels.Add(panel.panelID,panel);
    }

    public void ShowPanelStandAlone(string panelID)
    {
        if (panels.ContainsKey(panelID))
            panels[panelID].Show();
        else
            Debug.LogError($"Tried to show Panel Stand Alone but could not find {panelID}");
    }

    public void HidePanelStandAlone(string panelID)
    {
        if (panels.ContainsKey(panelID))
            panels[panelID].Hide();
        else
            Debug.LogError($"Tried to hide Panel Stand Alone but could not find {panelID}");
    }

    public void ShowPanel(string panelID)
    {
        ShowPanel(panelID, true);
    }

    public void ShowPanelNoBackStack(string panelID)
    {
        ShowPanel(panelID, false);
    }

    public void ShowPanel(string panelID, bool addToBackStack)
    {
        if(!panels.ContainsKey(panelID))
        {
            Debug.LogError($"Could not find {panelID} in the list of panels");
            return;
        }

        //Hide the previous panel
        if(currentPanel != null)
            currentPanel.Hide();

        //assign new one
        currentPanel = panels[panelID];

        //if we want to be able to go back to this then add to panel stack
        if (addToBackStack)
            panelStack.Push(panels[panelID]);

        //Show the current panel        
        currentPanel.Show();
    }

    public void Back()
    {
        if(currentPanel != null)
            currentPanel.Hide();

        //Then process the previous panel
        if (panelStack.Count > 0)
        {            
            //If the top of the stack is the current pannel
            if (panelStack.Peek() == currentPanel)
                panelStack.Pop();

            if (panelStack.Count > 0)
            {
                //Display the previous panel
                var previousPanel = panelStack.Pop();
                ShowPanel(previousPanel.panelID);
            }
            else
            {
                ShowPanel(DefaultPanelID);
            }                
        }
        else
        {
            ShowPanel(DefaultPanelID);
        }
    }

    public void AddCurrentPanelToBackStack()
    {
        if(panelStack.Count > 0)
        {
            //make sure to not push the same panel to the stack if the top is itself
            if(currentPanel != panelStack.Peek())
            {
                panelStack.Push(currentPanel);
            }
        }
        else 
        {
            panelStack.Push(currentPanel);
        }
    }
}
