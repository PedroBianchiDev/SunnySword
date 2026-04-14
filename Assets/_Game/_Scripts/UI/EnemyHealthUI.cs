using UnityEngine;
using UnityEngine.UI;
using SunnySword.Stats;

public class EnemyHealthUI : MonoBehaviour
{
    [Header("Configurações")]
    public Image hpBarFill;
    public GameObject healthBarRoot; 
    public bool hideIfFull = true;

    private StatsHandler stats;

    private void Awake()
    {
        stats = GetComponentInParent<StatsHandler>();
    }

    private void Start()
    {
        if (stats != null)
        {
            stats.OnStatsChanged += UpdateHealthBar;
            stats.OnDeath += HideBar;

            UpdateHealthBar();
        }
    }

    private void LateUpdate()
    {
        if (healthBarRoot != null)
        {
            healthBarRoot.transform.rotation = Quaternion.identity;
        }
    }

    private void UpdateHealthBar()
    {
        if (stats == null || hpBarFill == null) return;

        float hpPercent = stats.CurrentHealth / stats.Data.maxHealth;
        hpBarFill.fillAmount = hpPercent;

        if (hideIfFull && healthBarRoot != null)
        {
            healthBarRoot.SetActive(hpPercent < 1f && hpPercent > 0f);
        }
    }

    private void HideBar()
    {
        if (healthBarRoot != null) healthBarRoot.SetActive(false);
    }

    private void OnDestroy()
    {
        if (stats != null)
        {
            stats.OnStatsChanged -= UpdateHealthBar;
            stats.OnDeath -= HideBar;
        }
    }
}