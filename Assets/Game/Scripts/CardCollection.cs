using System;
using System.Collections;
using System.Collections.Generic;

public delegate void CardAddedEventHandler(object sender, CardCollectionEventArgs args);
public delegate void CardRemovedEventHandler(object sender, CardCollectionEventArgs args);
public class CardCollectionEventArgs : EventArgs
{
    public CardCollection Collection { get; }
    public Card Card { get; }

    public CardCollectionEventArgs(CardCollection collection, Card card)
    {
        Collection = collection;
        Card = card;
    }
}

public class CardCollection : IEnumerable<Card>
{
    public event CardAddedEventHandler Added;
    public event CardRemovedEventHandler Removed;

    protected List<Card> Cards { get; }

    public CardCollection()
    {
        Cards = new List<Card>();
    }

    public virtual void Add(Card card)
    {
        Added?.Invoke(this, new CardCollectionEventArgs(this, card));
        Cards.Add(card);
    }

    public virtual bool Remove(Card card)
    {
        Removed?.Invoke(this, new CardCollectionEventArgs(this, card));
        return Cards.Remove(card);
    }

    public virtual Card Pop()
    {
        Card top =  Cards.Count <= 0 ? default(Card) : Cards[Cards.Count - 1];
        Remove(top);

        return top;
    }

    public IEnumerator<Card> GetEnumerator()
    {
        return Cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}