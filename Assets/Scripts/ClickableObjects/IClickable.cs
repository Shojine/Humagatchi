using UnityEngine;

//Objects with this interface must have a collider in order to work
public interface IClickable
{
    public void OnClicked();

    public void OnHoverStart();
    public void OnHoverStop();
}
