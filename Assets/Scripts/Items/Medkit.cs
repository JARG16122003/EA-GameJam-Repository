using UnityEngine;

public class Medkit : ItemPickUp
{
    [SerializeField]
    private float amountHealthCure = 60.0f;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        ApplyLogicItem(collision.gameObject);
    }

    protected override void ApplyLogicItem(GameObject player)
    {
        
        IAttributes interfaceAttributes = player.GetComponent<IAttributes>();

        if (interfaceAttributes == null) return;

        interfaceAttributes.CureHealth(amountHealthCure);

        base.ApplyLogicItem(player);
    }

}
