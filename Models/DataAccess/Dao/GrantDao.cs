using Models.Common;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DataAccess
{
    public class GrantDao
    {
        #region Singleton

        /**
         * Constants
         */
        private ShopDbContext db = null;

        /**
         * @description -- init
         */

        private GrantDao()
        {
            db = new ShopDbContext();
        }

        private static GrantDao instance = null;

        public static GrantDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GrantDao();
                }
                return instance;
            }
        }

        #endregion Singleton

        #region Handle
        /**
         * @description -- get Grant by GrantID
         * @param _key: byte -- is field GrantID
         */

        public Grant getByID(byte _key)
        {
            return db.Grants.SingleOrDefault(x => x.GrantID == _key);
        }

        /**
         * @description -- insert a grant
         * @param _request: Grant -- entity object
         */

        public bool insert(Grant _request)
        {
            _request.isActive = false;
            _request.CreatedAt = DateTime.Now;
            db.Grants.Add(_request);
            db.SaveChanges();
            return true;
        }

        /**
         * @description -- change status a grant
         * @param _key: byte -- is field GrantID
         */

        public bool changeStatus(byte _key)
        {
            var grant = getByID(_key);
            grant.isActive = !grant.isActive;
            grant.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            return true;
        }

        /**
         * @description -- change new grant name
         * @param _request: GrantRequestDto -- is the data transmitted down from the display screen
         */

        public bool changeGrantName(GrantRequestDto _request)
        {
            var grant = getByID(_request.GrantID);
            grant.GrantName = _request.GrantName;
            grant.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            return true;
        }

        /**
         * @description -- get grant list by search key
         * @param _search: string -- is search key
         */

        public IEnumerable<Grant> getObjectList(string _search = null)
        {
            return _search != null ? db.Grants.Where(obj => obj.GrantName.Contains(_search) || obj.GrantID.ToString().Contains(_search)).ToList() : db.Grants.ToList();
        }

        #endregion Handle
    }
}