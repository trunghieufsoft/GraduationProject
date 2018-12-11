using Models.Common;
using Models.Common.Encode;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DataAccess
{
    public class CommentDao
    {
        #region Singleton
        /**
         * Constants 
         */
        private ShopDbContext db = null;

        /**
         * @description -- init
         */
        private CommentDao()
        {
            db = new ShopDbContext();
        }


        private static CommentDao instance = null;

        public static CommentDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommentDao();
                }
                return instance;
            }
        }
        #endregion Singleton
        
        #region Handle
        /**
         * @description -- get Comment by ComID
         * @param _key: byte -- is field ComID
         */
        public Comment getByID(string _key)
        {
            return db.Comments.SingleOrDefault(x => x.ComID == _key);
        }


        /**
         * @description -- get Comment list by product code
         * @param _prodCode: int
         */
        public List<Comment> GetComments(int _prodCode)
        {
            var comments = db.Comments.Include("Replies")
                .Where(x=> x.ProdId == _prodCode)
                .ToList();

            return comments;
        }

        /**
         * @description -- insert a Comment
         * @param _request: Comment -- entity object
         */
        public bool insert(Comment _request)
        {
            _request.ComID = Converter.genIdFormat_ddmmyy(db, Converter.ItemTypes.Comment);
            _request.CreatedAt = DateTime.Now;
            db.Comments.Add(_request);
            db.SaveChanges();
            return Constants.trueValue;
        }

        /**
         * @description -- delete a Comment
         * @param _key: byte -- is field ComID
         */
        public bool delete(string _key)
        {
            var comment = getByID(_key);
            if (hasReference(_key))
                return Constants.falseValue;
            db.Comments.Remove(comment);
            db.SaveChanges();
            return Constants.trueValue;
        }

        /**
         * @description -- change new context comment
         * @param _request: CommentRequestDto -- is the data transmitted down from the display screen
         */
        public bool changeContent(CommentRequestDto _request)
        {
            var Comment = getByID(_request.ComID);
            Comment.Content = _request.Content;
            Comment.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            return Constants.trueValue;
        }

        /**
         * @description -- get Comment list by search key
         * @param _search: string -- is search key content
         */
        public IEnumerable<Comment> getObjectList(string _search)
        {
            var model = db.Comments.ToList();
            if (_search != null)
            {
                model = model.Where(c => c.Content.ToLower().Contains(_search.ToLower()) ||
                               c.ComID.ToLower().Contains(_search.ToLower())).ToList();
            }
            return model;
        }


        /**
         * @description -- check has data reference to object
         * @param _key: string -- is field ComID
         */

        private bool hasReference(string _key)
        {
            var comment = getByID(_key);
            if (comment != default(Comment))
            {
                var count = db.Replies.Where(obj => obj.ComID == _key).ToList().Count;
                return count > Constants.zeroNumber ? Constants.trueValue : Constants.falseValue;
            }
            return Constants.falseValue;
        }

        #endregion Handle
    }
}
