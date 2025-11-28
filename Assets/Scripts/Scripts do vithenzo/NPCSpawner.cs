using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class NPCSpawner : MonoBehaviour
{
    [Header("Configuração NPC")]
    public Sprite[] possibleSprites;
    [Range(0f, 1f)]
    public float impostorChance = 0.05f; 

    [Header("Referências UI")]
    public Image npcImageUI; // aqui a UI Image que mostra o sprite use Canvas

    void Start()
    {
        if (possibleSprites == null || possibleSprites.Length == 0)
        {
            Debug.LogWarning("[NPCSpawner] Nenhum sprite configurado em possibleSprites!");
            return;
        }

        // escolher sprite aleatório
        int idx = Random.Range(0, possibleSprites.Length);
        Sprite chosen = possibleSprites[idx];

        // decidi se é impostor
        bool isImpostor = Random.value < impostorChance;

        // salvar no GameStats para uso posterior
        GameStats.currentNPCSprite = chosen;
        GameStats.currentNPCIsImpostor = isImpostor;

        // aplicar na UI
        if (npcImageUI != null)
            npcImageUI.sprite = chosen;

        Debug.Log($"[NPCSpawner] NPC gerado: spriteIndex={idx}, impostor={isImpostor}");
    }
}
