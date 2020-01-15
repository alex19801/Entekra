using Entekra.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Entekra.Services
{
    public interface IProjectService
    {
        Task GetProjectsFromExternalRepository();
    }
}
