using RPG.Dialogue;
using RPG.Shops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        GameObject player;
        Shopper shopper = null;
        Shop currentShop = null;

        private void OnEnable()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            shopper = player.GetComponent<Shopper>();

            ShopChanged();

            if (shopper == null) return;

            shopper.OnActiveShopChange += ShopChanged;
        }

        private void OnDisable()
        {
            shopper.OnActiveShopChange -= ShopChanged;
        }

        private void ShopChanged()
        {
            player.GetComponent<PlayerConversant>().Quit();
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);
        }
    }
}
