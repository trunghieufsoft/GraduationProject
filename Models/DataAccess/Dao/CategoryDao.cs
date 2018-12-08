using Models.Common;
using Models.Common.Encode;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DataAccess
{
    public class CategoryDao
    {
        /**
         * Constants
         */
        private ShopDbContext db = null;

        /**
         * @description -- init
         */

        private CategoryDao()
        {
            db = new ShopDbContext();
        }

        private static CategoryDao instance;

        public static CategoryDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoryDao();
                }
                return instance;
            }
        }

        

        /**
         * @description -- get Category by CategoryID
         * @param _keyCategory: byte -- is field CateID
         */

        public Category getByID(byte _keyCategory)
        {
            return db.Categories.SingleOrDefault(obj => obj.CateID == _keyCategory);
        }

        /**
         * @description -- check exits product in table Category
         * @param _cate: Category -- is a transion object
         */

        public bool hasCategory(Category _cate)
        {
            var category = db.Categories.SingleOrDefault(obj => obj.CateID == _cate.CateID || obj.CodeName.ToLower() == _cate.CodeName.ToLower());
            return category != default(Category) ? Constants.trueValue : Constants.falseValue;
        }

        /**
         * @description -- insert a Category
         * @param _request: Category -- entity object
         */

        public bool insert(Category _request)
        {
            _request.CodeName = Converter.genCode(_request.CateName);
            if (!hasCategory(_request))
            {
                _request.CreatedAt = DateTime.Now;
                db.Categories.Add(_request);
                db.SaveChanges();
                return Constants.trueValue;
            }
            return Constants.falseValue;
        }

        public List<Category> GetListCategory()
        {
            return db.Categories.ToList();
        }

        /**
         * @description -- delete a Category
         * @param _keyCategory: byte -- is field CateID
         */

        public bool delete(byte _keyCategory)
        {
            var category = getByID(_keyCategory);
            if (hasReference(_keyCategory))
                return Constants.falseValue;
            db.Categories.Remove(category);
            db.SaveChanges();
            return Constants.trueValue;
        }

        /**
         * @description -- change status
         * @param _request: CategoryRequestDto -- is the data transmitted down from the display screen
         */

        public bool changeStatus(byte id)
        {
            var category = getByID(id);
            category.isActive = !category.isActive;
            category.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            return category.isActive;
        }

        /**
         * @description -- change Category
         * @param _request: CategoryRequestDto -- is the data transmitted down from the display screen
         */

        public bool update(CategoryRequestDto _request)
        {
            var category = getByID(_request.CateID);
            category.CateName = _request.CateName;
            category.isActive = _request.isActive;
            category.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            return Constants.trueValue;
        }

        /**
         * @description -- get Category list by search key (CateID)
         * @param _key: string -- is search key
         */

        public IEnumerable<Category> getObjectList(string searchString = null)
        {
            var model = db.Categories.ToList();
            if (searchString != null)
            {
                model = model.Where(c => c.CateName.ToLower().Contains(searchString.ToLower()) || c.CodeName.Contains(searchString.ToLower())).ToList();
            }
            return model;
        }

        /**
         * @description -- check has data reference to object
         * @param _key: int -- is field CateID
         */

        private bool hasReference(byte _key)
        {
            var category = getByID(_key);
            if (category != default(Category))
            {
                var count = db.Products.Where(obj => obj.CateID == _key).ToList().Count;
                return count > Constants.zeroNumber ? Constants.trueValue : Constants.falseValue;
            }
            return Constants.falseValue;
        }

        public Category getByCode(string code)
        {
            return db.Categories.SingleOrDefault(x=> x.CodeName == code);
        }
    }
}