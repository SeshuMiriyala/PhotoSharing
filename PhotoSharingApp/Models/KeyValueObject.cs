// ***********************************************************************
// Assembly         : PhotoSharingApp
// Author           : Seshu Miriyala
// Created          : 09-19-2013
//
// Last Modified By : Seshu Miriyala
// Last Modified On : 09-19-2013
// ***********************************************************************
// <copyright file="KeyValueObject.cs" company="Coding Bugs">
//     Copyright (c) Coding Bugs. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Web.Http.ModelBinding;
using PhotoSharingApp.Models.ModelBinders;

namespace PhotoSharingApp.Models
{
    /// <summary>
    /// Class KeyValueObject.
    /// </summary>
    [ModelBinder(typeof(ModelBinder<KeyValueObject>))]
    public class KeyValueObject
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }
    }
}