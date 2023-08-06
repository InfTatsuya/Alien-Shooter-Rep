using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPurchaseListener
{
    public bool HandlePurchase(Object newPurchase);
}

public class CreditComponent : MonoBehaviour, IRewardListener
{
    public delegate void OnCreditChanged(int newCredits);
    public event OnCreditChanged onCreditChanged;

    [SerializeField] int credits;
    public int Credits => credits;

    [SerializeField] Component[] purchaseListeners;

    private List<IPurchaseListener> purchaseListenerInterfaces = new List<IPurchaseListener>();

    private void Start()
    {
        CollectPurchaseListeners();
    }

    private void CollectPurchaseListeners()
    {
        foreach(var listener in purchaseListeners)
        {
            IPurchaseListener purchaseListener = listener as IPurchaseListener;
            if(purchaseListener != null)
            {
                purchaseListenerInterfaces.Add(purchaseListener);
            }
        }
    }

    private void BroadcastPurchase(Object item)
    {
        foreach(var purchaseInterface in purchaseListenerInterfaces)
        {
            if (purchaseInterface.HandlePurchase(item)) return;
        }
    }


    public bool Purchase(int price, Object item)
    {
        if(credits < price) return false;

        credits -= price;
        onCreditChanged?.Invoke(credits);

        BroadcastPurchase(item);

        return true;
    }

    public void Reward(Reward reward)
    {
        credits += reward.creditReward;
        onCreditChanged?.Invoke(credits);
    }
}
