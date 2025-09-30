using UnityEngine;

public class SelectedCarHelper 
{
    public string SelectedCarId { get; private set; }

    public void SetId(string id)
    {
        SelectedCarId = id;
    }
}
