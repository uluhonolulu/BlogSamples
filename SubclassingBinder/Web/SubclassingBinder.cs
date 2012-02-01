using System;
using System.Web.Mvc;

namespace Web {
	public class SubclassingBinder : DefaultModelBinder {
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
			if (bindingContext.ValueProvider.ContainsPrefix("ModelType")) {
				var typeName = (string) bindingContext.ValueProvider.GetValue("ModelType").ConvertTo(typeof(string));
				var modelType = Type.GetType(typeName);
				bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, modelType);
			}
			return base.BindModel(controllerContext, bindingContext);
		}
	}
}