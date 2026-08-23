using UnityEngine;

public interface IHumanSubscriber
{
    public void updateHuman(HumanState state);
}


public interface IGameSubscriber
{
    public void updateMoney(int currentFunds);
}