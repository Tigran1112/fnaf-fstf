using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    public bool Over;
    private int night = 1;

    public UIDocument doc;
    private VisualElement newGame, contune, quit;

    void Awake()
    {
        var root = doc.rootVisualElement;
        newGame = root.Q<Label>("NewGame");
        contune = root.Q<Label>("Contune");
        quit = root.Q<Label>("Quit");
        if (!Over)
        {
            newGame.RegisterCallback<PointerEnterEvent>(evt => Hover(true, newGame));
            newGame.RegisterCallback<PointerLeaveEvent>(evt => Hover(false, newGame));

            contune.RegisterCallback<PointerEnterEvent>(evt => Hover(true, contune));
            contune.RegisterCallback<PointerLeaveEvent>(evt => Hover(false, contune));

            quit.RegisterCallback<PointerEnterEvent>(evt => Hover(true, quit));
            quit.RegisterCallback<PointerLeaveEvent>(evt => Hover(false, quit));

            newGame.RegisterCallback<PointerDownEvent>(evt => Newgame());
            contune.RegisterCallback<PointerDownEvent>(evt => Contune());
            quit.RegisterCallback<PointerDownEvent>(evt => Quit());
        }
        else Invoke("Return", 5f);
    }
    void Hover(bool i, VisualElement lab)
    {
        if (i) lab.style.fontSize = 30;
        else lab.style.fontSize = 20;
    }

    public void Newgame()
    {
        night = 1;
        PlayerPrefs.SetInt("Night", night);
        SceneManager.LoadScene("n" + night);
    }
    public void Contune()
    {
        SceneManager.LoadScene("n" + PlayerPrefs.GetInt("Night"));
    }
    public void Quit()
    {
        Application.Quit();
    }
    void Return()
    {
        SceneManager.LoadScene("Menu");
    }
}
