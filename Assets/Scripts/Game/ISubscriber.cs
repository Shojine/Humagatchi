using UnityEngine;

public interface IHumanSubscriber
{
    public void updateHuman(HumanState state);
}


public interface IGameSubscriber
{
    public void updateMoney(int currentFunds);
    public void updateTime(int hour, int minute, int day, AmPm amPm);
}