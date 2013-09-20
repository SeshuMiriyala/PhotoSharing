// ***********************************************************************
// Assembly         : PhotoSharingApp
// Author           : Seshu Miriyala
// Created          : 09-19-2013
//
// Last Modified By : Seshu Miriyala
// Last Modified On : 09-19-2013
// ***********************************************************************
// <copyright file="SignUpModel.cs" company="Coding Bugs">
//     Copyright (c) Coding Bugs. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel.DataAnnotations;
using System.Web.Http.ModelBinding;
using PhotoSharingApp.Models.ModelBinders;

namespace PhotoSharingApp.Models
{
    /// <summary>
    /// Class SignUpParams.
    /// </summary>
    [ModelBinder(typeof(ModelBinder<LoginParams>))]
    public class SignUpParams
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
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [Required]
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the middle name.
        /// </summary>
        /// <value>The name of the middle.</value>
        public string MiddleName { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [Required]
        public string LastName { get; set; }
    }
}