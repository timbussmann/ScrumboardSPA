using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScrumboardSPA.Controllers
{
    using ScrumboardSPA.Models;

    public class ScrumboardController : ApiController
    {
        public IEnumerable<ScrumboardColumn> GetColumns()
        {
            return new[]
                {
                    new ScrumboardColumn() {Name = "ToDo", Description = ""},
                    new ScrumboardColumn() {Name = "WIP", Description = ""},
                    new ScrumboardColumn() {Name = "To Verify", Description = ""},
                    new ScrumboardColumn() {Name = "Done", Description = ""}
                };
        }
    }
}
