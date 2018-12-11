using Models.Common;
using Models.Common.Encode;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DataAccess
{
    public class RatingDao
    {
        #region Singleton
        /**
         * Constants
         */
        private ShopDbContext db = null;

        /**
         * @description -- init
         */

        private RatingDao()
        {
            db = new ShopDbContext();
        }

        private static RatingDao instance;

        public static RatingDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RatingDao();
                }
                return instance;
            }
        }
        #endregion Singleton

        #region Handle
        /**
        * @description -- get Rating by RatID
        * @param _key: string -- is field RatID
        */
        public Rating getByID(string _key)
        {
            return db.Ratings.SingleOrDefault(obj => obj.RatID == _key);
        }

        /**
         * @description -- insert a Rating
         * @param _request: Rating -- entity object
         */

        public bool insert(Rating _request)
        {
            _request.RatID = Converter.genIdFormat_ddmmyy(db, Converter.ItemTypes.Rating);
            if (!hasRating(_request.RatID, _request.ProdID))
            {
                _request.CreatedAt = DateTime.Now;
                db.Ratings.Add(_request);
                db.SaveChanges();
                return Constants.trueValue;
            }
            return Constants.falseValue;
        }

        /**
         * @description -- delete a Rating
         * @param _key: string -- is field RatID
         */

        public bool delete(string _key)
        {
            db.Ratings.Remove(getByID(_key));
            db.SaveChanges();
            return Constants.trueValue;
        }

        /**
         * @description -- change level
         * @param _key: string -- is field RatID
         */

        public bool changeLevel(RatingRequestDto _request)
        {
            var Rating = getByID(_request.RatID);
            Rating.Level = _request.Level;
            Rating.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            return Constants.trueValue;
        }

        /// <summary>
        /// update info Rating case grant customer login and cusID not change
        /// </summary>
        /// <param name="_request"></param> -- is the data transmitted down from the display screen
        /// <returns></returns>
        public bool update(RatingRequestDto _request)
        {
            if (!hasRating(_request.UserID, _request.ProdID))
            {
                var Rating = getByID(_request.RatID);
                Rating.Level = _request.Level;
                Rating.ProdID = _request.ProdID;
                Rating.Content = _request.Content;
                db.SaveChanges();
                return Constants.trueValue;
            }
            return Constants.falseValue;
        }

        /// <summary>
        /// get Ratings list by search key
        /// </summary>
        /// <param name="_search"></param>
        /// <returns>IEnumerable<Rating></returns>
        public IEnumerable<Rating> getObjectList(string _search)
        {
            return db.Ratings.Where(obj => obj.RatID.Contains(_search) || obj.UserID.Contains(_search)).OrderBy(p => p.CreatedAt).ToList();
        }

        /**
         * @private
         * @description -- check the existence of image
         * @param
         */
        private bool hasRating(string _userID, int _prodID)
        {
            var rating = db.Ratings.SingleOrDefault(obj => obj.UserID == _userID &&  obj.ProdID == _prodID);
            return rating != default(Rating) ? Constants.trueValue : Constants.falseValue;
        }

        public IEnumerable<Rating> lsRatProd(int _prodID)
        {
            return db.Ratings.Where(x => x.ProdID == _prodID).OrderByDescending(x => x.CreatedAt);
        }

        /// <summary>
        /// get assessment by product code
        /// </summary>
        /// <returns>level assessment by percent</returns>
        public double getAssessment(int _idParam)
        {
            var assessment = 0.0;
            var lsRatProduct = lsRatProd(_idParam);
            foreach (var item in lsRatProduct)
            {
                assessment += item.Level;
            }
            return Math.Round(assessment / lsRatProduct.Count(), 2);
        }
        #endregion Handle
    }
}
