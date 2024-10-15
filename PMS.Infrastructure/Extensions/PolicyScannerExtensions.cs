using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PMS.Infrastructure.Extensions
{
    public static class PolicyScannerExtensions
    {
        public static List<string> GetAllPolicyNames(this Assembly assembly)
        {
            // This will hold all the policy names found in the project
            var policyNames = new List<string>();

            // Get all the types in the assembly
            var types = assembly.GetTypes();

            // Iterate through each type (usually controllers)
            foreach (var type in types)
            {
                // Get methods (which would be action methods in controllers)
                var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

                foreach (var method in methods)
                {
                    // Look for [Authorize] attributes
                    var authorizeAttributes = method.GetCustomAttributes<AuthorizeAttribute>();

                    foreach (var attr in authorizeAttributes)
                    {
                        if (!string.IsNullOrEmpty(attr.Policy) && !policyNames.Contains(attr.Policy))
                        {
                            // Add the policy name if it's not already added
                            policyNames.Add(attr.Policy);
                        }
                    }
                }
            }

            return policyNames;
        }
    }
}
