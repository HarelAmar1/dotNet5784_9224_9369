namespace BlApi;

static public class Factory
{
    public static IBl Get() => new BlImplementation.Bl();

}
