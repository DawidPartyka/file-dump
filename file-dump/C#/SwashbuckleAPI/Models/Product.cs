using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EanCode { get; set; }

        private Decimal price;
        public Decimal Price
        {
            get
            {
                return Decimal.Round(price, 2);
            }
            set
            {
                price = value;
            }
        }

        public class Example : IExamplesProvider<Product>
        {
            public Product GetExamples()
            {
                return new Product()
                {
                    Id = 123,
                    Name = "Orofar Max pastylki 20 past.",
                    EanCode = "5909990642571",
                    Price = 17.90m
                };
            }
        }

        public class Create
        {
            public string Name { get; set; }
            public string EanCode { get; set; }
            private Decimal price;
            public Decimal Price
            {
                get
                {
                    return Decimal.Round(price, 2);
                }
                set
                {
                    price = value;
                }
            }
            public class Example : IExamplesProvider<Create>
            {
                public Create GetExamples()
                {
                    return new Create()
                    {
                        Name = "Orofar Max pastylki 20 past.",
                        EanCode = "5909990642571",
                        Price = 17.90m
                    };
                }
            }

            public class ExampleList : IExamplesProvider<List<Create>>
            {
                public List<Create> GetExamples()
                {
                    List < Create > ret = new List<Create>();
                    ret.Add(new Create()
                    {
                        Name = "Orofar Max pastylki 20 past.",
                        EanCode = "5909990642571",
                        Price = 17.90m
                    });

                    return ret;
                }
            }
        }
    }
}
