using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{  
    public class MyPoint2d
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class MyPolyline
    {
        public List<MyPoint2d> Points { get; set; } = new List<MyPoint2d>();
    }
    public class MyLine
    {
        public MyPoint2d StartPoint { get; set; }
        public MyPoint2d EndPoint { get; set; }
    }

    public class MyCircle
    {
        public MyPoint2d Center { get; set; }
        public double Radius { get; set; }

        public class Create
        {
            public MyPoint2d Center { get; set; }
            public double Radius { get; set; }
        }
    }

    public class MyDoc
    {
        /// <summary>
        /// Id of a project
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of a project
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Date on which specific project was created
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// Date on which specific project was last modified
        /// </summary>
        public DateTime LastModificationTime { get; set; }
        /// <summary>
        /// Collection of Lines contained in a specific project
        /// </summary>
        public List<MyLine> Lines { get; set; }
        /// <summary>
        /// Collection of Polylines contained in a specific project
        /// </summary>
        public List<MyPolyline> Polylines { get; set; }
        /// <summary>
        /// Collection of Circles contained in a specific project
        /// </summary>
        public List<MyCircle> Circles { get; set; }

        public class Create
        {
            [Required]
            public string Name { get; set; }
            [Required]
            public List<MyLine> Lines { get; set; }
            [Required]
            public List<MyPolyline> Polylines { get; set; }
            [Required]
            public List<MyCircle> Circles { get; set; }

            
            public class Example : IExamplesProvider<Create>
            {
                public MyPoint2d PointEx(int x, int y)
                {
                    MyPoint2d p = new MyPoint2d();
                    p.X = x;
                    p.Y = y;
                    return p;
                }
                public MyLine LineEx()
                {
                    MyLine line = new MyLine();
                    line.StartPoint = PointEx(5, 6);
                    line.EndPoint = PointEx(6, 9);
                    return line;
                }
                public MyPolyline PolyEx()
                {
                    MyPolyline p = new MyPolyline();
                    p.Points.Add(PointEx(15, 4));
                    p.Points.Add(PointEx(0, 3));
                    return p;
                }
                public MyCircle CirEx()
                {
                    MyCircle c = new MyCircle();
                    c.Center = PointEx(4, 8);
                    c.Radius = 7.5;
                    return c;
                }

                public Create GetExamples()
                {
                    

                    return new Create()
                    {
                        Name = "My Project",
                        Lines = new List<MyLine>() { LineEx() },
                        Polylines = new List<MyPolyline>() { PolyEx() },
                        Circles = new List<MyCircle>() { CirEx() }
                    };
                }
            }
        }

        public class Update
        {
            public List<MyLine> Lines { get; set; }
            public List<MyPolyline> Polylines { get; set; }
            public List<MyCircle> Circles { get; set; }
        }
    }
}
