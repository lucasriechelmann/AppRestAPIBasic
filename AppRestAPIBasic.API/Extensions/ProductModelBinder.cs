using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using AppRestAPIBasic.Api.ViewModels;

namespace AppRestAPIBasic.API.Extensions
{
    public class ProductModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNameCaseInsensitive = true
            };

            var productViewModel = JsonSerializer.Deserialize<ProductImageViewModel>(bindingContext.ValueProvider.GetValue("produto").FirstOrDefault(), serializeOptions);
            productViewModel.ImageUpload = bindingContext.ActionContext.HttpContext.Request.Form.Files.FirstOrDefault();

            bindingContext.Result = ModelBindingResult.Success(productViewModel);
            return Task.CompletedTask;
        }
    }
}
