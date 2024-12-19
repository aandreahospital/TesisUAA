namespace SistemaBase.ModelsCustom
{
    public class JsonView
    {
        public string? ViewName { get; set; }
        public string? FormUrl { get; set; }
        public Method? Method { get; set; }
        public Tipo? Tipo { get; set; }
        public Windows? Windows { get; set; }
    }
    public class Windows
    {
        public Superior? Superior { get; set; }
        public Inferior? Inferior { get; set; }
    }
    public class Superior
    {
        public List<ElementsContainer>? Derecha { get; set; }
        public List<ElementsContainer>? Izquierdad { get; set; }
        public List<ElementsContainer>? Fila { get; set; }
    }
    public class Inferior
    {
        public List<ElementsContainer>? Derecha { get; set; }
        public List<ElementsContainer>? Izquierdad { get; set; }
        public List<ElementsContainer>? Fila { get; set; }
    }
    public class ElementsContainer
    {
        public string? Title { get; set; }
        public List<List<Element>>? Elements { get; set; }
    }
    public class Element
    {
        public string? ColClass { get; set; }
        public object[][]? Col { get; set; }
    }
    public enum Method
    {
        POST,
        GET,
        PUT,
        DELETE
    }
    public enum Tipo
    {
        A,
        B
    }
    public enum ElementsType
    {
        Input,
        Select,
        TextArea
    }
    public enum TypeInput
    {
        Normal,
        RadioButton,
        Checkbox,
    }
    public class Options
    {
        public string? label { get; set; }
        public string? value { get; set; }
    }
    public class Input
    {
        public string? Name { get; set; }
        public string? Id { get; set; }
        public string? LabelName { get; set; }
        public string? LabelClass { get; set; }
        public string? Class { get; set; }
        public TypeInput? Tipo { get; set; }
        public Options[]? options { get; set; }
    }
    public class Select
    {
        public string? Name { get; set; }
        public string? Id { get; set; }
        public string? LabelName { get; set; }
        public string? LabelClass { get; set; }
        public string? Class { get; set; }
        public Options[]? options { get; set; }
    }
    public class Div
    {
        public string? Id { get; set; }
        public string? Class { get; set; }
    }
}
