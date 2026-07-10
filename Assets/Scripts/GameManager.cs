using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Dados do Player")]
    public float VelocidadeMineiroBase;

    [Header("Informações da Lama")]
    public int LamaSlowDown;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // 1. Verifica se já existe uma instância e se não é esta
        if (Instance != null && Instance != this)
        {
            // Se já existe, destrua este objeto clone e pare por aqui!
            Destroy(gameObject);
            return;
        }

        // 2. Se não existe, esta passa a ser a instância oficial
        Instance = this;

        // 3. Diz para a Unity não destruir este objeto (e tudo grudado nele) ao trocar de cena
        DontDestroyOnLoad(gameObject);
    }

}
