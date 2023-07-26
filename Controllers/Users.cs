using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.S3;
using ConstructionApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Newtonsoft.Json.Linq;
using System.Linq;
using ConstructionApi.Contracts;
using System.Xml.Linq;

namespace ConstructionApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {
        private readonly DataDbContext _context;




        public Users( DataDbContext context)
        {
            _context = context;

        }


        [HttpGet]
        public async Task<IActionResult> GetUsers([FromHeader] int customerId, [FromQuery] string userPoolId)
        {
            var client = new AmazonCognitoIdentityProviderClient();
            var res = await client.ListUsersAsync( new ListUsersRequest() { UserPoolId = userPoolId } );
            if (res.HttpStatusCode== System.Net.HttpStatusCode.OK) {
                return Ok(res.Users); }
            else { return BadRequest(res); }
        }


        [HttpPost()]
        public async Task<IActionResult> CreateUser([FromHeader] int customerId, [FromBody] NewUSerObj userInfo)
        {
            var client = new AmazonCognitoIdentityProviderClient();
            var options = new List<AttributeType>(userInfo.Attributes);

            options.Add(new AttributeType() { Name = "custom:customerId", Value = customerId.ToString() });

            var request = new AdminCreateUserRequest() {
                UserPoolId = userInfo.UserPoolId, 
                TemporaryPassword = userInfo.Password,
                Username = userInfo.Username, 
                UserAttributes = options
            };

            var res = await client.AdminCreateUserAsync(request);

            
            if (res.HttpStatusCode == System.Net.HttpStatusCode.OK) { return Ok(res); }
            else
            {
                return BadRequest(res); 
            }
        }



        [HttpPut()]
        public async Task<IActionResult> EditUser([FromBody] AdminUpdateUserAttributesRequest request )
        {
            var client = new AmazonCognitoIdentityProviderClient();
            var res = await client.AdminUpdateUserAttributesAsync(request);
            if (res.HttpStatusCode == System.Net.HttpStatusCode.OK) { return Ok("Successfully Updated The User"); }
            else
            {
                return BadRequest(res);
            }
        }

        [HttpPost("enable")]
        public async Task<IActionResult> EnableUser([FromBody] AdminEnableUserRequest request)
        {
            var client = new AmazonCognitoIdentityProviderClient();
            var res = await client.AdminEnableUserAsync(request);
            if (res.HttpStatusCode == System.Net.HttpStatusCode.OK) { return Ok("Successfully Enabled The User"); }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost("disable")]
        public async Task<IActionResult> DisableUser( [FromBody] AdminDisableUserRequest request)
        {
            var client = new AmazonCognitoIdentityProviderClient();
            var res = await client.AdminDisableUserAsync( request);
            if (res.HttpStatusCode == System.Net.HttpStatusCode.OK) { return Ok("Successfully Disabled The User"); }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteUser( [FromQuery] string userPoolId,  string username)
        {
            var client = new AmazonCognitoIdentityProviderClient();
           var request= new AdminDeleteUserRequest() { Username = username, UserPoolId = userPoolId };
            var res = await client.AdminDeleteUserAsync(request);

            if (res.HttpStatusCode == System.Net.HttpStatusCode.OK) { return Ok("Successfully Deleted The User"); }
            else
            {
                return BadRequest();
            }
        }
    }











}
