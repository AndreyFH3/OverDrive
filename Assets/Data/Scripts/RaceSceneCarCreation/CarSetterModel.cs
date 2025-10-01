
public class CarSetterModel
{
    private SelectedCarHelper _helper;
    public string CarId => _helper.SelectedCarId;

    public CarSetterModel(SelectedCarHelper helper)
    {
        _helper = helper;
    }
    
}
