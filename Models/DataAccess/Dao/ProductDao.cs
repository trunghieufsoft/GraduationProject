using Models.Common;
using Models.Common.Encode;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.DataAccess
{
    public class ProductDao
    {

        #region Singleton
        /**
         * Constants
         */
        private readonly ShopDbContext db = null;

        /**
         * @description -- init
         */

        private ProductDao()
        {
            db = new ShopDbContext();
        }

        private static ProductDao instance = null;

        public static ProductDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductDao();
                }
                return instance;
            }
        }
        #endregion Singleton

        #region Handle

        public Product GetByCode(string code)
        {
            return db.Products.SingleOrDefault(x => x.Code == code);
        }

        public List<int> GetProductCount()
        {
            List<int> CateCounts = new List<int>();
            var products = db.Products.ToList();
            var categories = db.Categories.ToList();
            foreach (var itemCate in categories)
            {
                CateCounts.Add(products.Count(x => x.CateID == itemCate.CateID));
            }
            return CateCounts;
        }
        /**
         * @description -- get Product by ProdID
         * @param _key: int -- is field ProdID
         */

        public Product getByID(int _key)
        {
            return db.Products.SingleOrDefault(obj => obj.ProdID == _key);
        }

        public List<Product> HomeProductsPaging(int page, int pageSize, out int totalPages, out int totalRows)
        {
            var products = db.Products.Join(db.Categories, prod => prod.CateID, cate => cate.CateID, (prod, cate) => new { prod, cate })
                .Where(x => x.cate.isActive && x.prod.isActive)
                .Select(x => x.prod).OrderBy(x => x.CateID);
            
            totalRows = products.Count();
            totalPages = (int)Math.Ceiling((double)totalRows / pageSize);
            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        /**
         * @description -- check exits product in table Product
         * @param _prod: Product -- is a transion object
         */

        public bool hasProcuct(Product _prod)
        {
            var product = db.Products.SingleOrDefault(obj => obj.ProdID == _prod.ProdID || obj.Code == _prod.Code);
            return product != default(Product) ? true : false;
        }

        /**
         * @description -- insert a product
         * @param _request: Product -- entity object
         */

        public bool insert(Product _request)
        {
            _request.Code = Converter.genCode(_request.ProdName);
            if (!hasProcuct(_request))
            {
                _request.Image = Converter.imageToByteArray(
                    HttpContext.Current.Server.MapPath($"~/{_request.ImageUrl}"));
                _request.CreatedAt = DateTime.Now;
                db.Products.Add(_request);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        /**
         * @description -- delete a product
         * @param _key: int -- is field ProdID
         */

        public bool delete(int _key)
        {
            if (hasReference(_key))
                return false;
            db.Products.Remove(getByID(_key));
            db.SaveChanges();
            return true;
        }

        /**
         * @description -- change status active
         * @param _key: int -- is field ProdID
         */

        public bool changeStatus(int _key)
        {
            var product = getByID(_key);
            product.isActive = !product.isActive;
            product.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            return product.isActive;
        }

        /**
         * @description -- change new Image
         * @param _request: ProductRequestDto -- is the data transmitted down from the display screen
         */

        public bool changeImage(int productID, string imageUrl)
        {
            byte[] image = Converter.imageToByteArray(
                    HttpContext.Current.Server.MapPath($"~/{imageUrl}"));
            var product = getByID(productID);
            if (!hasImage(image))
            {
                product.Image = image;
                product.ImageUrl = imageUrl;
                product.UpdatedAt = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        /**
         * @description -- update info product has image
         * @param _request: ProductRequestDto -- is the data transmitted down from the display screen
         */

        public bool update(ProductRequestDto _request)
        {
            byte[] image = Converter.imageToByteArray(
                    HttpContext.Current.Server.MapPath($"~/{_request.ImageUrl}"));
            if (hasImage(image))
            {
                var product = getByID(_request.ProdID);
                product.Code = Converter.genCode(_request.ProdName);
                product.ProdName = _request.ProdName;
                product.Decription = _request.Decription;
                product.Cost = _request.Cost;
                product.Image = image;
                product.ImageUrl = _request.ImageUrl;
                product.UpdatedAt = DateTime.Now;
                product.isActive = _request.isActive;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        /**
         * @description -- update info product not image
         * @param _request: ProductRequestDto -- is the data transmitted down from the display screen
         */

        public bool UpdateExceptImage(ProductRequestDto _request)
        {
            var product = getByID(_request.ProdID);
            product.Code = Converter.genCode(_request.ProdName);
            product.ProdName = _request.ProdName;
            product.Decription = _request.Decription;
            product.Cost = _request.Cost;
            product.UpdatedAt = DateTime.Now;
            product.isActive = _request.isActive;
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// getObjectList
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="page"></param>
        /// <param name="totalRows"></param>
        /// <param name="totalPages"></param>
        /// <returns></returns>

        public IEnumerable<Product> getObjectList(string _search, int page, out int totalRows, out int totalPages)
        {
            var model = db.Products.OrderBy(p => p.CreatedAt).ToList();

            if (_search != null)
            {
                model = model.Where(obj => obj.ProdName.Contains(_search)).ToList();
            }

            totalRows = model.Count;
            totalPages = (int)Math.Ceiling((double)(totalRows / Constants.PageSize));

            return model.Skip((page - 1) * Constants.PageSize)
                        .Take(Constants.PageSize);
        }

        /// <summary>
        /// get products list by search key
        /// </summary>
        /// <param name="_search"></param>
        /// <returns>IEnumerable<Product></returns>

        public IEnumerable<Product> getObjectList(string _search)
        {
            return db.Products.Where(obj => obj.ProdName.Contains(_search) || obj.Code.Contains(_search)).OrderBy(p => p.CreatedAt).ToList();
        }

        /**
         * @private
         * @description -- check the existence of image
         * @param imagefilePath: string -- is the path of the image file
         */

        private bool hasImage(byte[] _image)
        {
            try
            {
                //finding the result in memory
                var result = db.Products.ToList();
                foreach (var item in result)
                {
                    if (item.Image != null)
                        if (item.Image.SequenceEqual(_image))
                            return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedAt).Take(top).ToList();
        }

        private bool hasReference(int _key)
        {
            var product = getByID(_key);
            if (product != default(Product))
            {
                var count_one = db.Orders.Where(obj => obj.ProdID == _key).ToList().Count;
                var count_two = db.Ratings.Where(obj => obj.ProdID == _key).ToList().Count;
                return (count_one + count_two) > 0;
            }
            return false;
        }

        public IEnumerable<Product> RelatedProducts(int id, int amount)
        {
            var product = this.getByID(id);
            var model = db.Products.Where(p => p.CateID == product.CateID)
                                    .OrderByDescending(p => p.CreatedAt)
                                    .Take(amount)
                                    .ToList();
            return model;
        }

        public IEnumerable<Product> RelatedProducts(int amount = 4)
        {
            var products = db.Products;
            var level = 0.0;
            var listlvProd = new List<any>();
            foreach (var element in products)
            {
                var listRating = db.Ratings.Where(rat => rat.ProdID == element.ProdID).ToList();
                if (listRating.Count > 0)
                {
                    foreach (var item in listRating)
                        level += item.Level;
                    listlvProd.Add(new any(){
                        product = element,
                        data = Math.Round(level/listRating.Count)
                    });
                }
            }
            return listlvProd.Count > 0 
                ? listlvProd.OrderByDescending(p => p.data).Select(p => p.product).Take(amount)
                : db.Products.Where(p => p.CateID == 1).OrderByDescending(p => p.CreatedAt).Take(amount);
        }

        public List<Product> GetByCategoryCode(string code)
        {
            return db.Products.Where(x => x.Category.CodeName == code).ToList();
        }

        public IEnumerable<Product> Recommendations(int amount = 4, bool isSearchByType = false, byte typeProd = 1)
        {
            if (amount < 4 || amount > 10)
            {
                amount = 4;
            }
            var lsObjPrdCount = new List<any>();
            var products = db.Products.Join(db.Categories, prod => prod.CateID, cate => cate.CateID, (prod, cate) => new { prod, cate })
                .Where(x => x.cate.isActive && x.prod.isActive)
                .Select(x => x.prod);
            products = isSearchByType ? products.Where(o => o.CateID == typeProd) : products;
            // step. 1: Get a list of favorite products
            foreach (var product in products)
            {
                var item = new any() {
                    product = product,
                    data = 0
                };
                foreach (var bill in db.Bills.Select(i => i.BillID))
                {
                    if (db.Orders.FirstOrDefault(i => i.BillID == bill && i.ProdID == product.ProdID) != default(Order))
                    {
                        item.data++;
                    }
                }
                lsObjPrdCount.Add(item);
            }
            // step. 2: Sort lsObjPrdCount get List product
            var Products = lsObjPrdCount.OrderByDescending(o => o.data).Select(l => l.product).ToList();
            // clear data source lsObjPrdCount
            lsObjPrdCount.Clear();
            // step. 3: Pick out the top 10 favorite products, with each favorite item, find in the 30 most recent orders, choose 4 most bought products
            foreach (var product in Products.Take(10))
            {
                var item = new any()
                {
                    product = product,
                    data = 0
                };
                var flag = Constants.RecommendationsNumber;
                foreach (var bill in db.Bills.OrderByDescending(i => i.CreatedAt).Select(i => i.BillID))
                {
                    if (flag == 0)
                        break;
                    flag--;
                    if (db.Orders.FirstOrDefault(i => i.BillID == bill && i.ProdID == product.ProdID) != default(Order))
                    {
                        item.data++;
                    }
                }
                lsObjPrdCount.Add(item);
            }
            // step. 4: return result recommendations
            return lsObjPrdCount.OrderByDescending(o => o.data).Select(o => o.product).Take(amount);
        }
        #endregion Handle
    }
    internal class any
    {
        public Product product { get; set; }
        public double data { get; set; }
    }
}