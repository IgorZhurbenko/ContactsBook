using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebForms
{
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var ContactInfo = DataManager.ContactInfo(id);
            if (ContactInfo == null)
            {
                return "Contact not found";
            }
            else
            {
                return System.Text.Json.JsonSerializer.Serialize(ContactInfo);
            }
        }

        // POST api/<controller>
        [HttpPost]
        public void Save([FromBody]object value)
        {
            Dictionary<string, object> Data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(value.ToString());
            if (Data["id"].ToBool())
            {
                DataManager.UpdateContactInfo(Data);
            }
            else 
            {
                DataManager.EnlistNewContact(Data);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            DataManager.DeleteContact(id);
        }
    }
}
