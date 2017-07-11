using System;
using System.Reflection;
using System.Windows.Input;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Axoom.MyService.Pipeline
{
    /// <summary>
    /// Bind HTTP request bodies to method parameters by default.
    /// </summary>
    public class CommandParameterBindingConvention : IActionModelConvention
    {
        /// <inheritdoc/>
        public void Apply(ActionModel action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (var parameter in action.Parameters)
            {
                if (typeof(ICommand).IsAssignableFrom(parameter.ParameterInfo.ParameterType))
                {
                    parameter.BindingInfo = parameter.BindingInfo ?? new BindingInfo();
                    parameter.BindingInfo.BindingSource = BindingSource.Body;
                }
            }
        }
    }
}