using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyVendor.MyService
{
    /// <summary>
    /// Specifies that a specific OAuth Scope is required to access this.
    /// </summary>
    public class ScopeAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Creates a new scope authorization attribute.
        /// </summary>
        /// <param name="scope">The name of the OAuth scope.</param>
        public ScopeAuthorizeAttribute(string scope)
            : base(scope)
        {}

        /// <summary>
        /// Lists the names of all scopes referenced by <see cref="ScopeAuthorizeAttribute"/>s in this assembly.
        /// </summary>
        public static ISet<string> GetAllScopes()
        {
            var scopes = new HashSet<string>();

            void AddScopes(MemberInfo element)
            {
                foreach (var attribute in element.GetCustomAttributes<ScopeAuthorizeAttribute>())
                    scopes.Add(attribute.Policy);
            }

            var controllers = Assembly.GetExecutingAssembly().GetTypes()
                                      .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Controller)))
                                      .ToList();

            foreach (var controller in controllers)
            {
                AddScopes(controller);
                foreach (var method in controller.GetMethods(BindingFlags.Instance | BindingFlags.Public))
                    AddScopes(method);
            }

            return scopes;
        }
    }
}
