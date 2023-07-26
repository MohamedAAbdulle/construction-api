using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Amazon.CognitoIdentityProvider.Model;

namespace ConstructionApi.Contracts
{
    public class UserAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class NewUSerObj
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string UserPoolId { get; set; }
        
        [Required]
        public List<AttributeType> Attributes { get; set; }

    }
}
