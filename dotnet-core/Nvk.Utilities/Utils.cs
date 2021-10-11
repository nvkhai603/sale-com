/*
 * Copyright (c) .NET Foundation and Contributors
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *
 * http://github.com/piranhacms/piranha
 *
 */

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nvk.Utilities
{
    /// <summary>
    /// Utility methods.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Clones the entire given object into a new instance.
        /// </summary>
        /// <param name="obj">The object to clone</param>
        /// <typeparam name="T">The object type</typeparam>
        /// <returns>The cloned instance</returns>
        public static T DeepClone<T>(T obj)
        {
            if (obj == null)
            {
                // Null value does not need to be cloned.
                return default(T);
            }

            if (obj is ValueType)
            {
                // Value types do not need to be cloned.
                return obj;
            }

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var json = JsonConvert.SerializeObject(obj, settings);

            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}
