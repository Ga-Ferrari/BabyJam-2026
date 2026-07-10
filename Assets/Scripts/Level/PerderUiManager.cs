using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDerrotaManager : MonoBehaviour
{
    [SerializeField] private string NomeCenaMenu;
    private CanvasGroup canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0f;
        canvas.interactable = false;  // Impede que o botão reaja a cliques
        canvas.blocksRaycasts = false;// Faz o clique do mouse "atravessar" o botão
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Recarregar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene(NomeCenaMenu);
    }

}
