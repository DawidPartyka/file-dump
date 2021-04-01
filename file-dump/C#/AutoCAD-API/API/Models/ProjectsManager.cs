using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ProjectsManager //: DbContext
    {
        /*public ProjectsManager(DbContextOptions<ProjectsManager> options) : base(options)
        {
        }*/
        
        private static ProjectsManager instance;
        private ProjectsManager() { }
        public static ProjectsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProjectsManager();
                }
                return instance;
            }
        }

        public List<MyDoc> Projects { get; set; } = new List<MyDoc>();
        //public DbSet<MyDoc> Projects { get; set; }
    }
}
