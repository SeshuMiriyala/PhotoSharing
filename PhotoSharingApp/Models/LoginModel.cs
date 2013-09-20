// ***********************************************************************
// Assembly         : PhotoSharingApp
// Author           : Seshu Miriyala
// Created          : 09-19-2013
//
// Last Modified By : Seshu Miriyala
// Last Modified On : 09-19-2013
// ***********************************************************************
// <copyright file="LoginModel.cs" company="Coding Bugs">
//     Copyright (c) Coding Bugs. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using PhotoSharingApp.Models.ModelBinders;
using System.ComponentModel.DataAnnotations;
using System.Web.Http.ModelBinding;

namespace PhotoSharingApp.Models
{
    /// <summary>
    /// Class LoginParams.
    /// </summary>
    [ModelBinder(typeof(ModelBinder<LoginParams>))]
    public class LoginParams
    {
        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        /// <value>The UserName.</value>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }
    }
}