using RSystems_CrudTask.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;

namespace RSystems_CrudTask.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public ProductApiController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [Route("GetProduct")] 
        [HttpGet]
        public List<Product> Get()
       {
            List<Product> products = new List<Product>();
            try
            {
                var rootPath = _hostingEnvironment.ContentRootPath; //get the root path
                var fullPath = Path.Combine(rootPath, "Data/product.json"); //combine the root path with that of our json file inside mydata directory
                var jsonData  = System.IO.File.ReadAllText(fullPath);
                if (string.IsNullOrWhiteSpace(jsonData)) return null; //if no data is present then return null or error if you wish
                else products = JsonConvert.DeserializeObject<List<Product>>(jsonData);

            }
            catch (Exception ex)
            {
                _logger.Error("Exception" + ex.Message);
                throw;
            }
            return products;
        }
        [Route("AddProduct")]
        [HttpPost]
        public bool InsertProduct(Product data)
        {
            try
            {
                var rootPath = _hostingEnvironment.ContentRootPath; //get the root path
                var fullPath = Path.Combine(rootPath, "Data/product.json");
                var jsonData = System.IO.File.ReadAllText(fullPath);
                List<Product> prods = new List<Product>();
                prods = JsonConvert.DeserializeObject<List<Product>>(jsonData);
                Product prod = prods.FirstOrDefault(x => x.Id == data.Id);
                if (prod == null)
                {
                    var id = prods.OrderBy(x => x.Id).LastOrDefault();
                    
                    data.Id = id.Id+1 ;
                    prods.Add(data);
                    //combine the root path with that of our json file inside mydata directory
                    var jsonString = JsonConvert.SerializeObject(prods, Formatting.Indented);
                    System.IO.File.WriteAllText(fullPath, jsonString);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
               _logger.Error("Exception" + ex);
                throw;
            }
            return true;
        }
        [Route("Update")]
        [HttpPut]
        public bool UpdateProduct(Product data)
        {
            try
            {
                var rootPath = _hostingEnvironment.ContentRootPath; //get the root path
                var fullPath = Path.Combine(rootPath, "Data/product.json");
                var jsonData = System.IO.File.ReadAllText(fullPath);
                List<Product> prods = new List<Product>();
                prods = JsonConvert.DeserializeObject<List<Product>>(jsonData);
                Product prod = prods.FirstOrDefault(x => x.Id == data.Id);
               
                    int index = prods.FindIndex(x => x.Id == data.Id);
                    prods[index] = data;
                
                //combine the root path with that of our json file inside mydata directory
                var jsonString = JsonConvert.SerializeObject(prods, Formatting.Indented);
                System.IO.File.WriteAllText(fullPath, jsonString);
            }
            catch (Exception ex)
            {
               _logger.Error("Exception" + ex);
                throw;
            }
            return true;
        }
        [Route("Delete/{id}")]
        [HttpDelete]
        public bool RemoveProduct(int Id)
        {
            try
            {
                var rootPath = _hostingEnvironment.ContentRootPath; //get the root path
                var fullPath = Path.Combine(rootPath, "Data/product.json");
                var jsonData = System.IO.File.ReadAllText(fullPath);
                List<Product> prods = new List<Product>();
                prods = JsonConvert.DeserializeObject<List<Product>>(jsonData);
                Product prod = prods.FirstOrDefault(x => x.Id == Id);

                prods.Remove(prods.Where(note => note.Id == Id).First());

                //combine the root path with that of our json file inside mydata directory
                var jsonString = JsonConvert.SerializeObject(prods, Formatting.Indented);
                System.IO.File.WriteAllText(fullPath, jsonString);
            }
            catch (Exception ex)
            {
               _logger.Error("Exception" + ex);
                throw;
            }
            return true;
        }
    }
}
