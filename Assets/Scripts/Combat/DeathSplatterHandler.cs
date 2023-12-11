using UnityEngine;

public class DeathSplatterHandler : MonoBehaviour
{
    private void OnEnable()
    {
        Health.OnDeath += SpawnDeathSplatter;
        Health.OnDeath += SpawnDeathVFX;
    }

    private void OnDisable()
    {
        Health.OnDeath -= SpawnDeathSplatter;
        Health.OnDeath -= SpawnDeathVFX;
    }

    private void SpawnDeathSplatter(Health sender)
    {
        GameObject splatterClone = Instantiate(
            sender.SplatterPrefab,
            sender.transform.position,
            sender.transform.rotation);

        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();

        if (splatterClone.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            if (colorChanger != null)
            {
                spriteRenderer.color = colorChanger.DefaultColor;
            }
        }

        splatterClone.transform.SetParent(this.transform);
    }

    private void SpawnDeathVFX(Health sender)
    {
        GameObject deathVFXClone = Instantiate(
            sender.DeathVFX,
            sender.transform.position,
            sender.transform.rotation);
        ParticleSystem.MainModule mainModule = deathVFXClone.GetComponent<ParticleSystem>().main;

        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();

        if (colorChanger != null)
        {
            mainModule.startColor = colorChanger.DefaultColor;
        }

        deathVFXClone.transform.SetParent(this.transform);
    }
}
