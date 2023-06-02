using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for controlling something via directional input only
/// </summary>
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

    /// <summary>
    /// Perform the submit action in the panel
    /// </summary>
    public void Submit();

}
