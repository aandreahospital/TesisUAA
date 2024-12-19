using Newtonsoft.Json;
using SistemaBase.ModelsCustom;
using System.ComponentModel;
namespace SistemaBase.Helpers
{
    public class ViewCreate: IViewCreate
    {
        public string ViewCreateHelper(JsonView _jsonview)
        {
            string template = "<div class='container'> " +
                               $"<h3>  {_jsonview?.ViewName} </h3>" +
                               $"<form method='{_jsonview?.Method}' action='{_jsonview?.FormUrl}' >";
            
            if(_jsonview?.Tipo== Tipo.A)
            {
                template = template + ViewPartA(_jsonview?.Windows);
            }
            template = template+ "</form> </div>";
            return template;
        }
        public string ViewPartA(Windows windows)
        {
            string template = "";
            if (windows?.Superior?.Fila != null) { 
            foreach (var superior in windows.Superior.Fila) {
            template = template + "<fieldset class='scheduler-border'>" +
                                   $"<legend class='scheduler-border'>Nombres:{superior?.Title}</legend>";
            template = template + ElementsCreate(superior);
            template = template + "</fieldset>";
            }
            }
            if (windows?.Inferior?.Fila != null)
            {
                foreach (var inferior in windows.Inferior.Fila)
                {
                    template = template + "<fieldset class='scheduler-border'>" +
                            $"<legend class='scheduler-border'>Nombres:{inferior?.Title}</legend>";
            template = template + ElementsCreate(inferior);
            template = template + "</fieldset>";
                }
            }
            return template;
        }
        public string ElementsCreate(ElementsContainer elementsContainer)
        {
            string template = "";
            if (elementsContainer?.Elements != null) { 
             foreach(var elements in elementsContainer.Elements) { 
            template = template + $"<div class='row'>";
            foreach (var container in elements)
            {
                template = template + $"<div class='{container.ColClass}'>";
                    template = template + $"<div class='row'>";
                    foreach (var element in container?.Col)
                {
                    if (element[0].ToString() == "Input")
                    {
                        if (element[2] != null)
                        {
                                //if (element[2].ToString().Contains("col"))
                                //{
                                //    template = template + $"<div class='row'>";
                                //}
                           template = template + $"<div class='{element[1]}'>";
                            var obj = JsonConvert.DeserializeObject<Input>(element[2].ToString());
                            template = template + inputcreate(obj!);
                            template = template + "</div>";
                            //if (element[2].ToString().Contains("Col"))
                            //{
                            //    template = template + "</div>";
                            //}
                        }
                    }
                }
                template = template + "</div>";
                template = template + "</div>";
            }
               template = template + "</div>";
            }
            }
            // template = template + "</div>";
            return template;
        }
        public string inputcreate(Input input)
        {
            string template = "";
            if (input.Tipo == TypeInput.Normal)
            {
                template = template + $"<label for='{input.Name}' class='{input.LabelClass}'>{input.LabelName}</label> " +
                    $"<input type='text' class='{input.Class}' id='{input.Id}'>";
            }
            else if (input.Tipo == TypeInput.Checkbox)
            {
                template = template + $" <div class='mb-3'>" +
                                        $"<label for='{input.Name}' class='{input.LabelClass}'>{input.LabelName}</label>";
                    foreach (var option in input.options)
                {
                    template = template + $"<div class='form-check'> " +
                                       $"<input class='form-check-input' type='checkbox' value='{option.value}'name='{input.Name}' id='{input.Id}'>" +
                                       $"<label class='form-check-label' for='{option.label}'>{option.label}" +
                                       $"</label>" +
                                       $"</div> ";
                }
                                       
         }
            return template;
        }
    }
}
