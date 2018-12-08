using Models.Common;
using Models.Common.Encode;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DataAccess.Dao
{
    public class RepliesDao
    {
        /**
         * Constants
         */
        private ShopDbContext db = null;

        /**
         * @description -- init
         */

        private RepliesDao()
        {
            db = new ShopDbContext();
        }

        private static RepliesDao instance;

        public static RepliesDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RepliesDao();
                }
                return instance;
            }
        }

        /**
         * @description -- get Replies by repNo
         * @param _key: int -- is field repNo
         */

        public Reply getByID(int _repNo, string _comID)
        {
            return db.Replies.SingleOrDefault(obj => obj.RepNo == _repNo && obj.ComID == _comID);
        }

        /**
         * @description -- insert a Replies
         * @param _request: Replies -- entity object
         */

        public bool insert(Reply _request)
        {
            _request.RepNo = Converter.genIdFormat_numberNo(db, _request.ComID);
            _request.CreatedAt = DateTime.Now;
            db.Replies.Add(_request);
            db.SaveChanges();
            return Constants.trueValue;
        }

        /**
         * @description -- delete a Replies
         * @param _key: string -- is field repNo
         */

        public bool delete(int _repNo, string _comID)
        {
            db.Replies.Remove(getByID(_repNo, _comID));
            db.SaveChanges();
            return Constants.trueValue;
        }

        /// <summary>
        /// update info Replies case grant customer login and cusID not change
        /// </summary>
        /// <param name="_request"></param> -- is the data transmitted down from the display screen
        /// <returns></returns>
        public bool update(RepliesRequestDto _request)
        {
            var Replies = getByID(_request.RepNo, _request.ComID);
            if (Replies == default(Reply))
                return Constants.falseValue;
            Replies.Content = _request.Content;
            db.SaveChanges();
            return Constants.trueValue;
        }

        /// <summary>
        /// get Repliess list by search key
        /// </summary>
        /// <param name="_search"></param>
        /// <returns>IEnumerable<Replies></returns>

        public IEnumerable<Reply> getObjectList(string _search)
        {
            return db.Replies.Where(obj => obj.ComID.Contains(_search) || obj.UserID.Contains(_search)).OrderBy(p => p.CreatedAt).ToList();
        }

        public List<Reply> ListNewReplies(int top)
        {
            return db.Replies.OrderByDescending(x => x.CreatedAt).Take(top).ToList();
        }
    }
}
