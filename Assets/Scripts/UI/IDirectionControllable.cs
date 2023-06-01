using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDirectionControllable
{
    /// <summary>
    /// Navigate up in the panel
    /// </summary>
    public void MoveUp();
    /// <summary>
    /// Navigate down in the panel
    /// </summary>
    public void MoveDown();
    /// <summary>
    /// Navigate left in the panel
    /// </summary>
    public void MoveLeft();
    /// <summary>
    /// Navigate right in the panel
    /// </summary>
    public void MoveRight();

}
