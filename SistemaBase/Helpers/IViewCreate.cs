using SistemaBase.ModelsCustom;
namespace SistemaBase.Helpers
{
    public interface IViewCreate
    {
        string ViewCreateHelper(JsonView _jsonview);
        string ViewPartA(Windows windows);
        string ElementsCreate(ElementsContainer elementsContainer);
        string inputcreate(Input input);
    }
}
