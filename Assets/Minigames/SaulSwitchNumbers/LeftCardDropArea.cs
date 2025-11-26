using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCardDropArea : MonoBehaviour, ICardDropArea
{
    [SerializeField] SaulSwitchManManager saulSwitchManManager;

    private Card currentCard;
    public void OnCardDrop(Card card)
    {
        if (currentCard == null)
        {
            currentCard = card;
            card.transform.position = transform.position;
            Debug.Log(card.name);
            if (card.name.ToString() == "One")
            {
                saulSwitchManManager.LeftCard = true;
            }
        }
        else
        {
            card.transform.position = card.GetStartPosition();
        }
    }
}
