using UnityEngine;

public class Ammo : ItemPickUp
{
    [SerializeField]
    private float ammountAmmo = 10.0f;

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
        IInventory interfaceInventory = player.GetComponent<IInventory>();

        if(interfaceInventory == null) return;

        interfaceInventory.IncreaseAmmo(ammountAmmo);

        base.ApplyLogicItem(player);
    }
}
