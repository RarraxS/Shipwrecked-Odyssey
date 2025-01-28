using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class CollectableObject : MonoBehaviour, IObserver
{
    [SerializeField] private bool startWithoutObject;

    public string requiredTool;
    [SerializeField] private int minimumToolLevel;
    public bool allowHitting;
    [SerializeField] private int energyPerHit;
    [SerializeField] private int healthPoints;

    public List<Drops> drops;
    [SerializeField] private float maximumSpawnDistance;


    [SerializeField] private bool seed;
    public int daysToGrow, totalDaysToGrow;
    public bool plowted, watered;
    private ParentVariablesColectableItems parentVars;


    public bool rotateOnCollisionEnter;
    [SerializeField] private bool ignoreTransparency;
    private bool observing;


    [SerializeField] private GameObject noCollisionItem;
    [SerializeField] private Color semitransparencyColor;


    private Transform transformComponent;

    public SpriteRenderer spriteRendererComponent;
    public PolygonCollider2D collisionComponent;
    public PolygonCollider2D isTriggerCollisionComponent;
    public Animator animatorComponent;


    [SerializeField] private List<ItemAndProbability> objectsToSpawn;

    private List<SpriteRenderer> spriteRendererComponentList = new List<SpriteRenderer>();
    private List<PickUpItem> pickUpItemComponentList = new List<PickUpItem>();


    private void Start()
    {
        GetAllComponents();

        if (startWithoutObject == true)
        {
            DeactivateObject();
        }

        for(int i = 0; i < drops.Count; i++)
        {
            SpriteRenderer _spriteRendererComponent = drops[i].dropItem.GetComponent<SpriteRenderer>();
            spriteRendererComponentList.Add(_spriteRendererComponent);

            PickUpItem _pickUpItemComponent = drops[i].dropItem.GetComponent<PickUpItem>();
            pickUpItemComponentList.Add(_pickUpItemComponent);
        }

        Observe();
    }

    private void GetAllComponents()
    {
        transformComponent = GetComponent<Transform>();

        spriteRendererComponent = GetComponent<SpriteRenderer>();
        collisionComponent = GetComponent<PolygonCollider2D>();
        isTriggerCollisionComponent = noCollisionItem.GetComponent<PolygonCollider2D>();
        animatorComponent = GetComponent<Animator>();

        parentVars = GetComponentInParent<ParentVariablesColectableItems>();
    }


    #region Hit objects
    public void ClasifyHit()
    {
        if (CanHit())
        {
            if (HaveAnyItemEquiped())
            {
                if (HasCorrectToolEquiped())
                    HitWithTool();

                else if (NoRequiredTool())
                    Hit(1);
            }

            else if (NoRequiredTool())
                Hit(1);

            else
                return;
        }
    }

    private bool CanHit()
    {
        return allowHitting == true && spriteRendererComponent.enabled == true;
    }

    private bool HaveAnyItemEquiped()
    {
        return Toolbar.Instance.herramientaSeleccionada.item != null;
    }

    private bool HasCorrectToolEquiped()
    {
        return Toolbar.Instance.herramientaSeleccionada.item.tool == requiredTool && 
               Toolbar.Instance.herramientaSeleccionada.item.tool != "";
    }

    private void HitWithTool()
    {
        Hit(Toolbar.Instance.herramientaSeleccionada.item.toolDamage);

        Jugador.Instance.energia -= Toolbar.Instance.herramientaSeleccionada.item.energyPerHit;
    }

    private bool NoRequiredTool()
    {
        return requiredTool == "";
    }

    public void Hit(int damage) 
    {
        healthPoints -= damage;

        CheckGenerateDrops();
    }

    private void CheckGenerateDrops()
    {
        if (healthPoints <= 0)
        {
            healthPoints = 0;

            GenerateDrops();
        }
    }

    #endregion

    #region Aparecer y desaparecer objetos
    private void GenerateDrops()
    {
        // Aqui tengo que hacer la pool de objetos para q no pete la memoria

        for (int dropsCount = 0; dropsCount < drops.Count; dropsCount++)
        {
            if (AreDifferentClassesOfDrops(dropsCount))
            {
                for (int probabilitiesCount = drops[dropsCount].probabilities.Count - 1; probabilitiesCount >= 0; probabilitiesCount--)
                {
                    int randomNumber = UnityEngine.Random.Range(1, 101);

                    if (randomNumber <= drops[dropsCount].probabilities[probabilitiesCount].probability)
                    {
                        for (int dropsQuantity = drops[dropsCount].probabilities[probabilitiesCount].quantity; dropsQuantity > 0; dropsQuantity--)
                        {
                            float distanciaAparicion;

                            distanciaAparicion = UnityEngine.Random.Range(0, maximumSpawnDistance);

                            Vector3 posicionAleatoria = (UnityEngine.Random.insideUnitSphere * distanciaAparicion) + transformComponent.position;
                            posicionAleatoria.z = 0;

                            GameObject droppedItem = ItemPool.Instance.RequestItem(posicionAleatoria);
                            
                            SpriteRenderer spriteRendererComponent = droppedItem.GetComponent<SpriteRenderer>();
                            spriteRendererComponent.sprite = spriteRendererComponentList[probabilitiesCount].sprite;

                            PickUpItem pickUpItemComponent = droppedItem.GetComponent<PickUpItem>();
                            pickUpItemComponent.item = pickUpItemComponentList[probabilitiesCount].item;
                            pickUpItemComponent.quantity = pickUpItemComponentList[probabilitiesCount].quantity;
                            pickUpItemComponent.cantidadBarraActual = pickUpItemComponentList[probabilitiesCount].cantidadBarraActual;
                            pickUpItemComponent.cantidadBarraMaxima = pickUpItemComponentList[probabilitiesCount].cantidadBarraMaxima;

                            //Instantiate(drops[dropsCount].dropItem, posicionAleatoria, Quaternion.identity);
                        }
                        break;
                    }
                }
            }
        }

        DeactivateObject();
    }

    private bool AreDifferentClassesOfDrops(int index)
    {
        return drops[index].dropItem != null;
    }








    private void DeactivateObject()
    {
        spriteRendererComponent.enabled = false;
        collisionComponent.isTrigger = true;
        animatorComponent.enabled = false;
    }

    public void CambiarAndAparecerObjeto(CollectableObject objeto)
    {
        spriteRendererComponent.sprite = objeto.spriteRendererComponent.sprite;

        // Variables sobre la herramienta para recolectar y los drops -----------------------------
        requiredTool = objeto.requiredTool;
        minimumToolLevel = objeto.minimumToolLevel;
        allowHitting = objeto.allowHitting;
        energyPerHit = objeto.energyPerHit;

        healthPoints = objeto.healthPoints;

        drops = objeto.drops;
        //-----------------------------------------------------------------------------------------

        // Variables para las semillas y el paso del tiempo ---------------------------------------
        seed = objeto.seed;
        totalDaysToGrow = objeto.totalDaysToGrow;
        //-----------------------------------------------------------------------------------------

        // Variables de interaccion fisica --------------------------------------------------------
        rotateOnCollisionEnter = objeto.rotateOnCollisionEnter;
        ignoreTransparency = objeto.ignoreTransparency;
        //-----------------------------------------------------------------------------------------

        // Variables para el resto de componentes -------------------------------------------------
        collisionComponent.points = objeto.collisionComponent.points;
        collisionComponent.isTrigger = objeto.collisionComponent.isTrigger;
        isTriggerCollisionComponent.points = objeto.isTriggerCollisionComponent.points;
        animatorComponent.runtimeAnimatorController = null;
        animatorComponent.runtimeAnimatorController = objeto.animatorComponent.runtimeAnimatorController;
        //-----------------------------------------------------------------------------------------

        spriteRendererComponent.enabled = true;
        collisionComponent.enabled = true;
        animatorComponent.enabled = true;

        if (seed == true)
        {
            daysToGrow = animatorComponent.GetInteger("dias");
        }

        RestablecerColor();
    }

    #endregion

    #region Alternar color

    public void TransparentarObjeto()
    {
        if (ignoreTransparency == false)
        {
            spriteRendererComponent.color = semitransparencyColor;
        }
    }

    public void RestablecerColor()
    {
        if (ignoreTransparency == false)
        {
            spriteRendererComponent.color = new Color(1, 1, 1, 1);
        }
    }

    #endregion

    private void ProbabilidadSpawnear()
    {
        float probabilidad = UnityEngine.Random.Range(0f,100f);

        float probabilidadTecho = 0;

        for (int i = 0; i < objectsToSpawn.Count; i++)
        {
            probabilidadTecho += objectsToSpawn[i].probability;

            if(probabilidad <= probabilidadTecho)
            {
                CambiarAndAparecerObjeto(objectsToSpawn[i].worldItem);

                return;
            }
        }
    }

    private void Observe()
    {
        ObserverManager.Instance.AddObserver(this);
        observing = true;
    }

    public void OnNotify(string eventInfo)
    {
        if (eventInfo == "Day completed")
        {
            if (spriteRendererComponent.enabled == false)
            {
                ProbabilidadSpawnear();


                if (watered == true)
                {
                    watered = false;
                }
            }

            if (watered == true)
            {
                if (seed == true)
                {
                    animatorComponent.SetInteger("dias", daysToGrow += 1);
                }

                watered = false;
            }
        }

        if (eventInfo == "Rainy day")
        {
            if (plowted == true)
            {
                Vector3Int position = Vector3Int.FloorToInt(transformComponent.position);
                position += new Vector3Int(-1, -1, 0);


                parentVars.tilemapWatered.SetTile(position, parentVars.tileWatered);
            }
        }
    }
}

[Serializable]
public class ItemAndProbability
{
    public CollectableObject worldItem;

    public float probability;
}

[Serializable]
public class Drops
{
    public GameObject dropItem;
    
    public List<QuantityAndProbabilities> probabilities;
}

[Serializable]
public class QuantityAndProbabilities
{
    public int quantity;

    public float probability;
}