using System;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace PhotoSharingApp.Models.ModelBinders
{
    public class ModelBinder<T> : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(T))
            {
                return false;
            }

            var stream = actionContext.Request.Content.ReadAsStringAsync().Result;

            if (stream == null)
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName, "Wrong value type");
                return false;
            }

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginParams>(Encoding.Default.GetString(Convert.FromBase64String(stream)));

            if (null != result)
            {
                bindingContext.Model = result;
                return true;
            }

            bindingContext.ModelState.AddModelError(
                bindingContext.ModelName, "Cannot extract credentials");
            return false;
        }
    }
}