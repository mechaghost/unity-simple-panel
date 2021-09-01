using UnityEngine;

public class SimplePanel : MonoBehaviour
{
    public SimplePanelManager simplePanelManager;
    public GameObject rootElement;
    public string panelID;

    public void Show()
    {
        rootElement.SetActive(true);
    }

    public void Hide()
    {
        rootElement.SetActive(false);
    }

    private void Awake()
    {
        //Register yourself
        simplePanelManager.AddPanel(this);

        //Turn yourself off by default
        rootElement.SetActive(false);
    }    
}
